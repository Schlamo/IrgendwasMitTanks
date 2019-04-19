using UnityEngine;

using Enumerators;

namespace PowerUps {
    public class PowerUp : MonoBehaviour {
        protected float     timer    = 0.0f;
        private bool        exploded = false;

        public float        bonus    = 12.5f;
        public float        lifetime = 0.0f;
        public PowerUpType  type = PowerUpType.SpeedUp;
        public Gradient     gradient;

        public void Start() {
            timer = lifetime;
        }

        public void Update(){
            timer -= Time.deltaTime;

            if (!exploded && timer <= 0.5f) {
                exploded = true;

                ProjectileManager.instance.CreateExplosion(
                    transform.position, new Gradient[]
                    { gradient, gradient },
                    ExplosionSize.Medium
                );

                ProjectileManager.instance.CreateExplosion(
                    transform.position, new Gradient[]
                    { gradient, gradient },
                    ExplosionSize.Large
                );
            }
            else if (timer <= 0.0f)
                Destroy(gameObject);
        }
    }
}
