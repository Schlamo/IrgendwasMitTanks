using UnityEngine;
using UnityEngine.UI;
using System;
using Obstacles;
using PowerUps;
using Enumerators;
using Controllers;
using System.Linq;
using System.Collections.Generic;

public abstract class Tank : MonoBehaviour {
    public ITankController Controller { get; set; }

    public PlayerIndex PlayerIndex { get; set; }

    public float powerUpDuration = 30.0f;
    
    public float ramDamage = 0.0f;

    public float defaultSpeed = 15;
    public float speed = 15;
    public float maxSpeed = 30;
    private float speedGained = 0.0f;

    public float shootDelay = 0.5f;

    public float defaultDamage = 10.0f;
    public float damage = 10.0f;
    public float maxDamage = 50.0f;
    private float damageGained = 0.0f;

    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float defaultArmor = 1.0f;
    public float armor = 1.0f;
    public float maxArmor = 5.0f;

    private float armorGained = 0.0f;

    public float rotationSpeed = 50.0F;

    public float nitro;

    public Transform cam;
    public Transform canvas;
    public Transform launchPosition;

    public Sprite specialImage;

    private float canShoot = 0.0f;

    public bool CanDrive { get; set; }

    public float CurrentSpeed { get; private set; }

    public int Kills { get; private set; } = 0;
    public int Deaths { get; private set; } = 0;

    public PlayerIndex LastDamage { get; set; }
    private bool Died { get; set; } = false;
    public float nitroAmp = 1.5f;

    private float timeToExplode = 0.0f;
    private float funtime = 2.5f;

    GamePad.Index idx;
    XInputDotNetPure.PlayerIndex xIdx;
    // Use this for initialization

    public void Start()
    {
        transform.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        SetSpecialImage();
        idx  = (GamePad.Index)PlayerIndex;
        xIdx = (XInputDotNetPure.PlayerIndex)PlayerIndex;
    }

    public void Update () {
        //canDrive = CheckDistanceToTheMap( 0.5f);
        if (health <= 0.0f && !Died)
        {
            Explode();
            Died = true;
            return;
        }

        UpdateSpecial(Time.deltaTime, idx);

        if (CanDrive && transform.position.y > -10
            && Mathf.Abs((transform.rotation.eulerAngles.x + 22.5f) % 360) < 45.0f
            && Mathf.Abs((transform.rotation.eulerAngles.z + 22.5f) % 360) < 45.0f)
        {

            timeToExplode = 0.0f;
            float acceleration = Controller.Acceleration();

            if (Controller.Nitro() && nitro > 0)
            {
                acceleration *= nitroAmp;
                nitro -= Time.deltaTime;
            }

            if (Controller.Shoot() && canShoot <= 0.0f)
            {
                Shoot(launchPosition);
                GetComponent<Rigidbody>().AddForce(launchPosition.position);
                transform.Find("ShootParticles").gameObject.GetComponent<ParticleSystem>().Emit(5);
                canShoot = shootDelay;
                acceleration -= 10.0f;
            }
            else
            {
                canShoot -= Time.deltaTime;
            }

            var rb = gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * speed * acceleration * Time.deltaTime * 60, ForceMode.Force);

            CurrentSpeed = (rb.velocity.magnitude);
            transform.Rotate(0, CurrentRotationSpeed(), 0);

            ParticleSystem left = this.transform.Find("TracksParticlesLeft").gameObject.GetComponent<ParticleSystem>();
            ParticleSystem right = this.transform.Find("TracksParticlesRight").gameObject.GetComponent<ParticleSystem>();

            left.Emit((int)CurrentSpeed / 10);
            right.Emit((int)CurrentSpeed / 10);
        }
        else
        {
            timeToExplode += Time.deltaTime;
            if(timeToExplode > 3.0f)
            {
                health = 0.0f;
            }
        }

        /* Unique Abilities */
        if(GamePad.GetButtonDown(GamePad.Button.B, idx))
            Special();

        /* PowerUp Timers */
        if (speed > defaultSpeed)
        {
            speedGained += Time.deltaTime;
            if (speedGained > powerUpDuration)
            {
                speed = defaultSpeed;
                speedGained = 0.0f;
            }
        }

        if (damage > defaultDamage)
        {
            damageGained += Time.deltaTime;
            if (damageGained > powerUpDuration)
            {
                damage = defaultDamage;
                damageGained = 0.0f;
            }
        }

        if (armor > defaultArmor)
        {
            armorGained += Time.deltaTime;
            if (armorGained > powerUpDuration)
            {
                armor = defaultArmor;
                armorGained = 0.0f;
            }
        }

        if (health < 25)
            transform.Find("BurningParticles").gameObject.GetComponent<ParticleSystem>().Emit((int)Mathf.Sqrt(25 - health));

        UpdateStats();
    }

    private float CurrentRotationSpeed() => Controller.Rotation() * rotationSpeed * Time.deltaTime * (Controller.Nitro() ? 0.5f : 1.0f);

    public abstract void Special();

    public abstract void UpdateSpecialStats();

    public abstract void UpdateSpecial(float dTime, GamePad.Index idx);

    public void SetSpecialImage()
    {
        var canvas = transform.Find("Canvas");
        canvas.Find("Image_Special").GetComponent<Image>().sprite = specialImage;
    }

    public void Shoot(Transform pos) 
        => ProjectileManager.instance.CreateProjectile(transform, pos, damage, PlayerIndex);

    public void TakeDamage(float damage)
    {
        //StartVibration();
        Invoke("StopVibration", 0.25f);
        float actualDamage = damage - this.armor;
        this.health -= Mathf.Max(actualDamage, 0.0f);
    }

    public void TakeTrueDamage(float damage)
    {
        //StartVibration();
        Invoke("StopVibration", 0.25f);
        this.health -= damage;
    }

    public void Explode()
    {
        timeToExplode = 0.0f;
        GameManager.GiveKillToPlayer(LastDamage);
        ProjectileManager.instance.CreateExplosion(gameObject.transform.position, ExplosionType.Tank);
        Deaths++;
        Invoke("Respawn", 0.75f);  
    }

    private void Respawn()
    {
        var rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.Euler(Vector3.zero);
        health = maxHealth;
        Initialize(GameManager.GetStartingPoint(PlayerIndex));
    }

    public void Initialize(Vector3 pos)
    {
        armor        = defaultArmor;
        damage       = defaultDamage;
        speed        = defaultSpeed;
        armorGained  = 0.0f;
        speedGained  = 0.0f;
        damageGained = 0.0f;

        transform.position = new Vector3(pos.x, 0, pos.z);
        transform.rotation = Quaternion.Euler(0, pos.y, 0);

        cam.transform.position = new Vector3(pos.x, 0, pos.z);
        cam.transform.rotation = Quaternion.Euler(0, pos.y, 0);
        transform.LookAt(new Vector3(0, transform.position.y, 0));
    }

    public void UpdateStats()
    {
        var canvas = transform.Find("Canvas");
        canvas.Find("Health").GetComponent<Text>().text = Mathf.Max(Mathf.Round(health), 0).ToString();
        canvas.Find("Kills").GetComponent<Text>().text = Kills.ToString();
        canvas.Find("Nitro").GetComponent<Text>().text = Math.Round(nitro, 1).ToString();
        canvas.Find("Damage").GetComponent<Text>().text = Math.Round(damage, 0).ToString();
        canvas.Find("Armor").GetComponent<Text>().text = Math.Round(armor, 0).ToString();
        UpdateSpecialStats();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collider.gameObject.GetComponent<PowerUp>();

            AudioManager.instance.PlayPowerUpSound(powerUp.type);
            switch (powerUp.type)
            {
                case PowerUpType.SpeedUp:
                    speedGained = 0.0f;
                    speed += powerUp.bonus;
                    speed = speed > maxSpeed ? maxSpeed : speed;
                    break;
                case PowerUpType.DamageUp:
                    damageGained = 0.0f;
                    damage += powerUp.bonus;
                    damage = damage > maxDamage ? maxDamage : damage;
                    break;
                case PowerUpType.ArmorUp:
                    armorGained = 0.0f;
                    armor += powerUp.bonus;
                    armor = armor > maxArmor ? maxArmor : armor;
                    break;
                case PowerUpType.RepairKit:
                    Debug.Log("Healed by: " + (powerUp.bonus / 100) * (maxHealth-health));
                    health += (powerUp.bonus / 100) * (maxHealth - health);
                    health = health > maxHealth ? maxHealth : health;
                    break;
                case PowerUpType.Nitro:
                    nitro += powerUp.bonus;
                    GameManager.DeleteNitro(powerUp);
                    break;

            }
            Destroy(collider.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            Invoke("DisableDrive", funtime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
            CanDrive = true;
    }

    private void DisableDrive() => CanDrive = false;

    public void StartVibration()
    {
        XInputDotNetPure.GamePad.SetVibration(xIdx, 0.25f, 0.25f);
    }

    public void StopVibration()
    {
        XInputDotNetPure.GamePad.SetVibration(xIdx, 0.0f, 0.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tank")
        {
            AudioManager.instance.PlayTankCollisionSound();
            try
            {
                collision.transform.GetComponent<Tank>().LastDamage = PlayerIndex;
                float totalSpeed = (CurrentSpeed + collision.gameObject.GetComponent<Tank>().CurrentSpeed) / 2;
                TakeDamage(totalSpeed * (ramDamage / 100));
            }
            catch (System.Exception) { }
        }
        else if(collision.gameObject.tag == "Map")
        {
            CanDrive = true;
        }
        else if(collision.gameObject.tag == "Rock")
        {
            AudioManager.instance.PlayRockCollisionSound();
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            TakeTrueDamage((ramDamage / 100) * rb.velocity.magnitude);
        }
        else if (collision.gameObject.CompareTag("Tree"))
        {
            AudioManager.instance.PlayCrateCollisionSound();
            ProjectileManager.instance.CreateExplosion(collision.gameObject.transform.position, ExplosionType.Wood);
            var rb = gameObject.GetComponent<Rigidbody>();
            TakeDamage((ramDamage / 100) * rb.velocity.magnitude * 0.5f);
            rb.velocity *= 0.5f;
        }
        else if (collision.gameObject.CompareTag("Crate"))
        {
            AudioManager.instance.PlayCrateCollisionSound();
            collision.gameObject.GetComponent<Crate>().Hit(GetPercentageHealth());
            ProjectileManager.instance.CreateExplosion(collision.gameObject.transform.position, ExplosionType.Wood);
            var rb = gameObject.GetComponent<Rigidbody>();
            TakeDamage((ramDamage / 100) * rb.velocity.magnitude * 0.2f);
            rb.velocity *= 0.5f;
        }
    }

    public float CanShoot { get; set; }

    public void Paint (TankColor color)
    {
        Material mat = Resources.Load("Colors/" + color.ToString()) as Material;
        
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Colorized"))
                child.GetComponent<Renderer>().material = mat;
        }
    }

    public void AddKill() => Kills++;
    public float GetPercentageHealth() => (health / maxHealth) * 100.0f;
}
