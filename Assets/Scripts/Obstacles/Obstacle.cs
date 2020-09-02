using UnityEngine;

namespace Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public Gradient explosionGradientA;
        public Gradient explosionGradientB;

        public float initialScaling { get; set;}
        public float initialHP;
        protected float currentHP      { get; set;}

        public bool isDestroyable = true;
        public bool isBurning     = false;

        protected void Start() 
            => currentHP = initialHP;

        protected abstract void Destroy();

        public virtual void Hit(float damage) {
            if (!isDestroyable)
                return;

            currentHP -= damage;
        }

        protected virtual void Update() {
            if (isBurning)
                currentHP -= Time.deltaTime * 2;

            if (currentHP <= 0)
                Destroy();
        }
    }
}
