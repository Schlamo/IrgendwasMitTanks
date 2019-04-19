using UnityEngine;

namespace Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public Gradient explosionGradientA;
        public Gradient explosionGradientB;

        public  float initialScaling;

        public  float initialHP;
        protected float currentHP;

        public bool isDestroyable = true;
        public bool isBurning     = false;

        private void Start() {
            currentHP = initialHP;
        }

        protected abstract void Destroy();

        public void Hit(float damage) {
            if (!isDestroyable)
                return;

            currentHP -= damage;
        }

        public void Update() {
            if (isBurning)
                currentHP -= Time.deltaTime * 2;

            if (currentHP <= 0)
                Destroy();
        }
    }
}
