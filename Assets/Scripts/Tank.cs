using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Tank : MonoBehaviour {
    public int padNumber;

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

    public bool canDrive = false;

    private float currentSpeed;

    private int kills = 0;
    private int deaths = 0;

    private string name = "";

    private int lastDamage = -1;
    private bool died = false;
    public float nitroAmp = 1.5f;

    private float timeToExplode = 0.0f;

    GamePad.Index idx;
    XInputDotNetPure.PlayerIndex xIdx;
    // Use this for initialization

    private void PaintTank(int color){}

    void Start()
    {
        transform.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        SetSpecialImage();



        switch (padNumber)
        {
            case 1:
                idx = GamePad.Index.One;
                xIdx = XInputDotNetPure.PlayerIndex.One;
                break;
            case 2:
                idx = GamePad.Index.Two;
                xIdx = XInputDotNetPure.PlayerIndex.Two;
                break;
            case 3:
                idx = GamePad.Index.Three;
                xIdx = XInputDotNetPure.PlayerIndex.Three;
                break;
            case 4:
                idx = GamePad.Index.Four;
                xIdx = XInputDotNetPure.PlayerIndex.Four;
                break;
            default:
                idx = GamePad.Index.Any;
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            if(Mathf.Abs(Vector3.Distance(hit.point, transform.position)) > 0.5f)
            {
                canDrive = false;
            }
            else
            {
                canDrive = true;
            }
        }


        if (health <= 0.0f && !died)
        {
            Explode();
            died = true;
            return;
        }

        UpdateSpecial(Time.deltaTime, idx);

        float rightTrigger = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, idx);
        float leftTrigger = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, idx);
        float actualRotSpeed;

        actualRotSpeed = rotationSpeed;

        if (GamePad.GetButton(GamePad.Button.X, idx))
        {
            actualRotSpeed *= 0.5f;
        }

        if (canDrive && transform.position.y > -10 && Mathf.Abs((transform.rotation.eulerAngles.x+22.5f)%360) < 45.0f && Mathf.Abs((transform.rotation.eulerAngles.z + 22.5f) % 360) < 45.0f)
        {

            timeToExplode = 0.0f;
            float triggerDifference = rightTrigger - leftTrigger;

            if (GamePad.GetButton(GamePad.Button.X, idx) && nitro > 0)
            {
                triggerDifference *= nitroAmp;
                nitro -= Time.deltaTime;
            }

            if (GamePad.GetButton(GamePad.Button.A, idx) && canShoot <= 0.0f)
            {
                Shoot(this.launchPosition);
                GetComponent<Rigidbody>().AddForce(this.launchPosition.position);
                this.transform.Find("ShootParticles").gameObject.GetComponent<ParticleSystem>().Emit(5);
                canShoot = shootDelay;
                triggerDifference -= 10.0f;
            }
            else 
            {
                canShoot -= Time.deltaTime;
            }

            var rb = this.gameObject.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * speed * triggerDifference * Time.deltaTime * 60, ForceMode.Force);

            this.currentSpeed = (rb.velocity.magnitude);



            Vector2 leftAxis = GamePad.GetAxis(GamePad.Axis.LeftStick, idx);

            float rot = leftAxis.x * actualRotSpeed * Time.deltaTime;
            this.transform.Rotate(0, rot, 0);

            ParticleSystem left = this.transform.Find("TracksParticlesLeft").gameObject.GetComponent<ParticleSystem>();
            ParticleSystem right = this.transform.Find("TracksParticlesRight").gameObject.GetComponent<ParticleSystem>();

            left.Emit((int)currentSpeed / 10);
            right.Emit((int)currentSpeed / 10);
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
        {
            Special();
        }


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
        {
            int particles = (int)Mathf.Sqrt(25 - health);
            this.transform.Find("BurningParticles").gameObject.GetComponent<ParticleSystem>().Emit(particles);
        }

        UpdateStats();
    }

    public abstract void Special();

    public void SetSpecialImage()
    {
        var canvas = transform.Find("Canvas");
        canvas.Find("Image_Special").GetComponent<Image>().sprite = specialImage;
    }

    public abstract void UpdateSpecialStats();

    public abstract void UpdateSpecial(float dTime, GamePad.Index idx);

    public void Shoot(Transform pos)
    {
        ProjectileManager.instance.createProjectile(this.transform, pos, this.damage, this.padNumber);
    }

    public void TakeDamage(float damage)
    {
        StartVibration();
        Invoke("StopVibration", 0.25f);
        float actualDamage = damage - this.armor;
        this.health -= Mathf.Max(actualDamage, 0.0f);
    }

    public void TakeTrueDamage(float damage)
    {
        StartVibration();
        Invoke("StopVibration", 0.25f);
        this.health -= damage;
    }

    public void Explode()
    {
        timeToExplode = 0.0f;
        var rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.Euler(Vector3.zero);
        GameManager.instance.GiveKillToPlayer(lastDamage);
        ProjectileManager.instance.createExplosion(this.gameObject.transform.position, 3);
        deaths++;
        Invoke("Respawn", 0.75f);  
    }

    private void Respawn()
    {
        health = maxHealth;
        Spawn(GameManager.instance.GetSpawnPoint(padNumber));
    }

    public void Spawn(Vector3 pos)
    {
        died = false;
        armor = defaultArmor;
        damage = defaultDamage;
        speed = defaultSpeed;
        lastDamage = -1;
        armorGained = 0.0f;
        speedGained = 0.0f;
        damageGained = 0.0f;

        this.transform.position = new Vector3(pos.x, 0, pos.z);
        this.transform.rotation = Quaternion.Euler(0, pos.y, 0);

        cam.transform.position = new Vector3(pos.x, 0, pos.z);
        cam.transform.rotation = Quaternion.Euler(0, pos.y, 0);
        transform.LookAt(new Vector3(0, transform.position.y, 0));
    }

    public void UpdateStats()
    {
        var canvas = transform.Find("Canvas");
        canvas.Find("Health").GetComponent<Text>().text = Mathf.Max(Mathf.Round(health), 0).ToString();
        canvas.Find("Kills").GetComponent<Text>().text = kills.ToString();
        canvas.Find("Nitro").GetComponent<Text>().text = Math.Round(nitro, 1).ToString();
        canvas.Find("Damage").GetComponent<Text>().text = Math.Round(damage, 0).ToString();
        canvas.Find("Armor").GetComponent<Text>().text = Math.Round(armor, 0).ToString();
        UpdateSpecialStats();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            PowerUp powerUp = other.gameObject.GetComponent<PowerUp>();

            AudioManager.instance.PlayPowerUpSound(powerUp.type);
            switch (powerUp.type)
            {
                case 1:
                    this.speedGained = 0.0f;
                    this.speed += powerUp.bonus;
                    speed = speed > maxSpeed ? maxSpeed : speed;
                    break;
                case 2:
                    this.damageGained = 0.0f;
                    this.damage += powerUp.bonus;
                    damage = damage > maxDamage ? maxDamage : damage;
                    break;
                case 3:
                    this.armorGained = 0.0f;
                    this.armor += powerUp.bonus;
                    armor = armor > maxArmor ? maxArmor : armor;
                    break;
                case 4:
                    Debug.Log("Healed by: " + (powerUp.bonus / 100) * (maxHealth-health));
                    this.health += (powerUp.bonus / 100) * (maxHealth - health);
                    health = health > maxHealth ? maxHealth : health;
                    break;
                case 5:
                    this.nitro += powerUp.bonus;
                    GameManager.instance.DeleteNitro(powerUp);
                    break;

            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Crate")
        {
            AudioManager.instance.PlayCrateCollisionSound();
            other.gameObject.GetComponent<Crate>().Hit(currentSpeed, GetPercentageHealth());
            ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            TakeDamage((ramDamage / 100) * rb.velocity.magnitude *0.2f);
            rb.velocity *= 0.5f;
        }
        else if (other.gameObject.tag == "Tree")
        {
            AudioManager.instance.PlayCrateCollisionSound();
            var tree = other.gameObject.GetComponent<Tree>();
            tree.Hit(currentSpeed);
            ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            TakeDamage((ramDamage / 100) * rb.velocity.magnitude * 0.5f);
            rb.velocity *= 0.5f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            canDrive = false;
        }
    }

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
                collision.transform.GetComponent<Tank>().LastDamage = padNumber;
                float totalSpeed = (currentSpeed + collision.gameObject.GetComponent<Tank>().currentSpeed) / 2;
                TakeDamage(totalSpeed * (ramDamage / 100));
            }
            catch (System.Exception) { }
        }
        else if(collision.gameObject.tag == "Map")
        {
            canDrive = true;
        }
        else if(collision.gameObject.tag == "Rock")
        {
            AudioManager.instance.PlayRockCollisionSound();
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            TakeTrueDamage((ramDamage / 100) * rb.velocity.magnitude);
        }
    }

    public int Kills
    {
        get
        {
            return kills;
        }
        set
        {
            kills = value;
        }
    }

    public int LastDamage
    {
        get
        {
            return lastDamage;
        }
        set
        {
            lastDamage = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public float CanShoot
    {
        get
        {
            return canShoot;
        }
        set
        {
            canShoot = value;
        }
    }

    public void Paint (int color)
    {
        Material mat;
        string path = "Colors/";
        switch(color)
        {
            case 0:
                path += "red";
                break;
            case 1:
                path += "blue";
                break;
            case 2:
                path += "green";
                break;
            case 3:
                path += "yellow";
                break;
            case 4:
                path += "pink";
                break;
            case 5:
                path += "orange";
                break;
            case 6:
                path += "black";
                break;
            case 7:
                path += "white";
                break;
        }
        mat = Resources.Load(path) as Material;

        foreach (Transform child in transform)
        {
            if (child.tag == "Colorized")
            {
                child.GetComponent<Renderer>().material = mat;
            }
        }
    }

    public float GetPercentageHealth()
    {
        return (health / maxHealth)*100.0f;
    }
}
