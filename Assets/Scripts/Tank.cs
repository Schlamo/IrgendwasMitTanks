using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tank : MonoBehaviour {

    public float powerUpDuration = 30.0f;

    public float ramDamage = 0.0f;

    public float defaultSpeed = 15;
    public float speed = 15;
    public float maxSpeed = 30;
    private float speedGained = 0.0f;

    public float defaultShootDelay = 0.5f;
    public float shootDelay = 0.5f;
    public float minShootDelay = 0.1f;
    private float shootDelayGained = 0.0f;

    public float defaultDamage = 10.0f;
    public float damage = 10.0f;
    public float maxDamage = 50.0f;
    private float damageGained = 0.0f;

    public float health = 100.0f;
    public float maxHealth = 100.0f;
    private float healthGained = 0.0f;

    public float defaultArmor = 1.0f;
    public float armor = 1.0f;
    public float maxArmor = 5.0f;
    private float armorGained = 0.0f;

    public float rotationSpeed = 50.0F;
    public int padNumber;

    public float nitro;

    public Transform cam;
    public Transform launchPosition;

    private float canShoot = 0.0f;

    private bool canDrive = false;

    private float currentSpeed;

    private int kills = 0;
    private int deaths = 0;

    private float timeToExplode = 0.0f;

    // Use this for initialization
    void Start ()
    {
        UiManager.UpdateHP(this.padNumber, (int)Mathf.Round(health));
    }

    private void PaintTank(int color){}
	
	// Update is called once per frame
	void Update () {
        UpdateSpecial(Time.deltaTime);

        GamePad.Index idx;

        switch (padNumber)
        {
            case 1:
                idx = GamePad.Index.One;
                break;
            case 2:
                idx = GamePad.Index.Two;
                break;
            default:
                idx = GamePad.Index.Any;
                break;
        }

        float rightTrigger = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, idx);
        float leftTrigger = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, idx);

        if (GamePad.GetButtonDown(GamePad.Button.X, idx))
        {
            this.rotationSpeed *= 0.5f;
        }

        if (GamePad.GetButtonUp(GamePad.Button.X, idx))
        {
            this.rotationSpeed *= 2.0f;
        }

        /* Tank Movements */
        if (canDrive && Mathf.Abs((transform.rotation.eulerAngles.x+1.0f)%360) < 2.0f && Mathf.Abs((transform.rotation.eulerAngles.z + 1.0f) % 360) < 2.0f)
        {

            timeToExplode = 0.0f;
            float triggerDifference = rightTrigger - leftTrigger;

            if (GamePad.GetButton(GamePad.Button.X, idx) && nitro > 0)
            {
                triggerDifference *= 1.5f;
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
            rb.AddForce(transform.forward * speed * triggerDifference, ForceMode.Force);

            this.currentSpeed = (rb.velocity.magnitude);



            Vector2 leftAxis = GamePad.GetAxis(GamePad.Axis.LeftStick, idx);

            float rot = leftAxis.x * rotationSpeed * Time.deltaTime;
            this.transform.Rotate(0, rot, 0);

            ParticleSystem left = this.transform.Find("DirtParticlesLeft").gameObject.GetComponent<ParticleSystem>();
            ParticleSystem right = this.transform.Find("DirtParticlesRight").gameObject.GetComponent<ParticleSystem>();

            left.Emit((int)currentSpeed / 10);
            right.Emit((int)currentSpeed / 10);

        } else
        {
            timeToExplode += Time.deltaTime;
            if(timeToExplode > 10.0f)
            {
                Explode();
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
    }

    public abstract void Special();

    public abstract void UpdateSpecial(float dTime);

    public void Shoot(Transform pos)
    {
        ProjectileManager.instance.createProjectile(this.transform, pos, this.damage, this.padNumber);
    }

    public void TakeDamage(float damage)
    {
        float actualDamage = damage - this.armor;
        this.health -= Mathf.Max(actualDamage, 0.0f);
        UpdateStats();
        if(this.health <= 0.0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Player" + this.padNumber + " lost");
        ProjectileManager.instance.createExplosion(this.gameObject.transform.position, 3);
        //Destroy(this.cam.gameObject);
        Destroy(this.gameObject, 0.1f);
        //GameManager.ShowEndScreen();
    }

    public void Spawn(Vector3 pos)
    {
        armor = defaultArmor;
        damage = defaultDamage;
        speed = defaultSpeed;
        health = maxHealth;
        shootDelay = defaultShootDelay;

        this.transform.position = new Vector3(pos.x, 0, pos.z);
        this.transform.rotation = Quaternion.Euler(0, pos.y, 0);

        cam.transform.position = new Vector3(pos.x, 0, pos.z);
        cam.transform.rotation = Quaternion.Euler(0, pos.y, 0);
    }

    private void UpdateStats()
    {
        UiManager.UpdateHP(this.padNumber, (int)Mathf.Round(health));
        UiManager.UpdateKills(this.padNumber, kills);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            PowerUp powerUp = other.gameObject.GetComponent<PowerUp>();

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
                    this.health += powerUp.bonus;
                    health = health > maxHealth ? maxHealth : health;
                    break;
                case 5:
                    this.nitro += powerUp.bonus;
                    GameManager.instance.DeleteNitro(powerUp);
                    break;

            }

            UpdateStats();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Crate")
        {
            TakeDamage(5*(ramDamage/100));
            other.gameObject.GetComponent<Crate>().Hit(currentSpeed);
            ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            rb.velocity *= 0.25f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            canDrive = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tank")
        {
            try
            {
                float totalSpeed = (currentSpeed + collision.gameObject.GetComponent<Tank>().currentSpeed) / 2;
                health -= totalSpeed * (ramDamage / 100);
            }
            catch (System.Exception) { }
            UpdateStats();
        }
        else if(collision.gameObject.tag == "Map")
        {
            canDrive = true;
        }
    }
}
