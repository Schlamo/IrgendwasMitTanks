using UnityEngine;
using UnityEngine.UI;
using System;

using PowerUps;
using Controllers;
using Obstacles;
using Enumerators;

namespace Tanks {
    public abstract class Tank : MonoBehaviour {
        public Transform cam;
        public Transform canvas;
        public Transform launchPosition;
        public Sprite specialImage;

        public ITankController controller;
        public TankStats stats;
        public int padNumber;
        GamePad.Index idx;
        XInputDotNetPure.PlayerIndex xIdx;

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
        public float canShoot = 0.0f;
        public bool canDrive = false;
        private float currentSpeed;
        public int kills = 0;
        private int deaths = 0;
        private string name = "";
        public int lastDamage = -1;
        private bool died = false;
        public float nitroAmp = 1.5f;
        private float timeToExplode = 0.0f;

        private float test = 0.0f;

        public void Start() {
            transform.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
            SetSpecialImage();

            switch (padNumber) {
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

        public void Update () {
            float actualRotSpeed = rotationSpeed;
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();

            UpdateSpecial(Time.deltaTime, idx);
            
            if (health <= 0.0f && !died) {
                Explode();
                died = true;
                return;
            }
            
            if (GamePad.GetButton(GamePad.Button.X, idx))
                actualRotSpeed *= 0.5f;

            if (!canDrive)
                test += Time.deltaTime;

            if (test <0.5f && Mathf.Abs((transform.rotation.eulerAngles.x+22.5f)%360) < 45.0f 
                           && Mathf.Abs((transform.rotation.eulerAngles.z + 22.5f) % 360) < 45.0f) {
                timeToExplode = 0.0f;

                float acceleration = this.controller.Acceleration();
                float rotation     = this.controller.Rotation();

                if (this.controller.Nitro() && nitro > 0) {
                    acceleration *= nitroAmp;
                    nitro -= Time.deltaTime;
                }

                if (this.controller.Shoot() && canShoot <= 0.0f) {
                    Shoot(this.launchPosition);
                    GetComponent<Rigidbody>().AddForce(this.launchPosition.position);
                    this.transform.Find("ShootParticles").gameObject.GetComponent<ParticleSystem>().Emit(5);
                    canShoot = shootDelay;
                    acceleration -= 2.5f;
                }
                else 
                    canShoot -= Time.deltaTime;

                rb.AddForce(transform.forward * speed * acceleration * Time.deltaTime * 60, ForceMode.Force);

                this.currentSpeed = (rb.velocity.magnitude);

                Vector2 leftAxis = GamePad.GetAxis(GamePad.Axis.LeftStick, idx);

                this.transform.Rotate(0, controller.Rotation() * actualRotSpeed * Time.deltaTime, 0);

                ParticleSystem left = this.transform.Find("TracksParticlesLeft").gameObject.GetComponent<ParticleSystem>();
                ParticleSystem right = this.transform.Find("TracksParticlesRight").gameObject.GetComponent<ParticleSystem>();

                left.Emit((int)currentSpeed / 5);
                right.Emit((int)currentSpeed / 5);
            } else {
                timeToExplode += Time.deltaTime;

                if (timeToExplode > 3.0f)
                    health = 0.0f;
            }

            // PowerUp Timers
            if (speed > defaultSpeed) {
                speedGained += Time.deltaTime;

                if (speedGained > powerUpDuration) {
                    speed = defaultSpeed;
                    speedGained = 0.0f;
                }
            }

            if (damage > defaultDamage) {
                damageGained += Time.deltaTime;

                if (damageGained > powerUpDuration) {
                    damage = defaultDamage;
                    damageGained = 0.0f;
                }
            }

            if (armor > defaultArmor) {
                armorGained += Time.deltaTime;

                if (armorGained > powerUpDuration) {
                    armor = defaultArmor;
                    armorGained = 0.0f;
                }
            }

            if (health < 25) {
                int particles = (int)Mathf.Sqrt(25 - health);
                transform.Find("BurningParticles").gameObject.GetComponent<ParticleSystem>().Emit(particles);
            }

            UpdateStats();
        }

        public abstract void Special();

        public abstract void UpdateSpecialStats();

        public abstract void UpdateSpecial(float dTime, GamePad.Index idx);

        public void SetSpecialImage() {
            transform.Find("Canvas").Find("Image_Special").GetComponent<Image>().sprite = specialImage;
        }

        public void Shoot(Transform pos) {
            ProjectileManager.instance.createProjectile(transform, pos, damage, padNumber);
        }

        public void TakeDamage(float damage) {
            health -= Mathf.Max(damage - armor, 0.0f);

            StartVibration();
            Invoke("StopVibration", 0.25f);
        }

        public void TakeTrueDamage(float damage) {
            health -= damage;

            StartVibration();
            Invoke("StopVibration", 0.25f);
        }

        public void Explode() {
            var rb = transform.GetComponent<Rigidbody>();

            timeToExplode = 0.0f;
            rb.velocity = Vector3.zero;
            rb.rotation = Quaternion.Euler(Vector3.zero);
            GameManager.instance.GiveKillToPlayer(lastDamage);
            ProjectileManager.instance.createExplosion(this.gameObject.transform.position, 3);
            deaths++;
            Invoke("Respawn", 0.75f);  
        }

        private void Respawn() {
            health = maxHealth;
            Spawn(GameManager.instance.GetSpawnPoint(padNumber));
        }

        public void Spawn(Vector3 pos) {
            died         = false;
            armor        = defaultArmor;
            damage       = defaultDamage;
            speed        = defaultSpeed;
            lastDamage   = -1;
            armorGained  = 0.0f;
            speedGained  = 0.0f;
            damageGained = 0.0f;

            transform.position = new Vector3(pos.x, 0, pos.z);
            transform.rotation = Quaternion.Euler(0, pos.y, 0);

            cam.transform.position = new Vector3(pos.x, 0, pos.z);
            cam.transform.rotation = Quaternion.Euler(0, pos.y, 0);

            transform.LookAt(new Vector3(0, transform.position.y, 0));
        }

        public void UpdateStats() {
            var canvas = transform.Find("Canvas");
            canvas.Find("Health").GetComponent<Text>().text = Mathf.Max(Mathf.Round(health), 0).ToString();
            canvas.Find("Kills").GetComponent<Text>().text = kills.ToString();
            canvas.Find("Nitro").GetComponent<Text>().text = Math.Round(nitro, 1).ToString();
            canvas.Find("Damage").GetComponent<Text>().text = Math.Round(damage, 0).ToString();
            canvas.Find("Armor").GetComponent<Text>().text = Math.Round(armor, 0).ToString();
            UpdateSpecialStats();
        }

        void OnTriggerEnter(Collider other) {
            var powerUp = other.GetComponent<PowerUp>();

            if (powerUp != null) {
                AudioManager.instance.PlayPowerUpSound(powerUp.type);

                switch (powerUp.type) {
                    case PowerUpType.SpeedUp:
                        speedGained = 0.0f;
                        speed += powerUp.bonus;
                        break;
                    case PowerUpType.DamageUp:
                        damageGained = 0.0f;
                        damage += powerUp.bonus;
                        break;
                    case PowerUpType.ArmorUp:
                        armorGained = 0.0f;
                        armor += powerUp.bonus;
                        break;
                    case PowerUpType.RepairKit:
                        health += (powerUp.bonus / 100) * (maxHealth - health);
                        break;
                    case PowerUpType.Nitro:
                        nitro += powerUp.bonus;
                        break;
                }

                armor  = armor  > maxArmor  ? maxArmor  : armor;
                speed  = speed  > maxSpeed  ? maxSpeed  : speed;
                damage = damage > maxDamage ? maxDamage : damage;
                health = health > maxHealth ? maxHealth : health;

                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Tree") {
                AudioManager.instance.PlayCrateCollisionSound();
                var tree = other.gameObject.GetComponent<Obstacles.Tree>();
                tree.Hit(currentSpeed);
                ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
                Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
                TakeDamage((ramDamage / 100) * rb.velocity.magnitude);
                rb.velocity *= 0.25f;
            }
        }

        void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.tag == "Obstacle") {
                try {
                    Obstacle  obstacle = collision.gameObject.GetComponent<Obstacle>();
                    Rigidbody rb       = gameObject.GetComponent<Rigidbody>();

                    AudioManager.instance.PlayRockCollisionSound();
                    TakeTrueDamage((ramDamage / 100) * rb.velocity.magnitude);

                    if (obstacle.isDestroyable)
                        obstacle.Hit(currentSpeed/2);

                    ProjectileManager.instance.CreateExplosion(
                        collision.contacts[0].point,
                        new Gradient[] { obstacle.explosionGradientA, obstacle.explosionGradientB },
                        ExplosionSize.Medium);

                }
                catch (System.Exception e) {
                    Debug.LogError(e.StackTrace);
                }
            }

            if (collision.gameObject.tag == "Tank") {
                AudioManager.instance.PlayTankCollisionSound();

                try {
                    collision.transform.GetComponent<Tank>().lastDamage = padNumber;
                    float totalSpeed = (currentSpeed + collision.gameObject.GetComponent<Tank>().currentSpeed) / 2;
                    TakeDamage(totalSpeed * (ramDamage / 100));
                }
                catch (Exception) {}
            }
            else if(collision.gameObject.tag == "Map") {
                canDrive = true;
                test = 0.0f;
            }
            else if(collision.gameObject.tag == "Rock") {
                AudioManager.instance.PlayRockCollisionSound();
                Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
                TakeTrueDamage((ramDamage / 100) * rb.velocity.magnitude);
            }
        }

        private void OnCollisionExit(Collision collision) {
            if (collision.gameObject.tag == "Map")
                canDrive = false;
        }

        public void StartVibration() {
            XInputDotNetPure.GamePad.SetVibration(xIdx, 0.25f, 0.25f);
        }

        public void StopVibration() {
            XInputDotNetPure.GamePad.SetVibration(xIdx, 0.0f, 0.0f);
        }

        public void Paint (int color) {
            Material mat;
            string path = "Colors/";
            switch(color) {
                case 0: path += "red";      break;
                case 1: path += "blue";     break;
                case 2: path += "green";    break;
                case 3: path += "yellow";   break;
                case 4: path += "pink";     break;
                case 5: path += "orange";   break;
                case 6: path += "black";    break;
                case 7: path += "white";    break;
            }
            mat = Resources.Load(path) as Material;

            foreach (Transform child in transform)
                if (child.tag == "Colorized")
                    child.GetComponent<Renderer>().material = mat;
        }

        public float GetPercentageHealth() {
            return (health / maxHealth)*100.0f;
        }
    }
}