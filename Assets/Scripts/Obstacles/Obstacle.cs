using UnityEngine;

namespace Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public Gradient explosionGradientA;
        public Gradient explosionGradientB;

        public float initialHP   = 15.0f;
        public bool  destroyable = true;
        public bool  burnable    = false;

        public    float InitialScaling { get; set; }
        protected float CurrentHP      { get; set; }
        private   bool IsBurning       { get; set; }

        protected void Start() 
            => CurrentHP = initialHP;

        protected abstract void Destroy();

        public virtual void Hit(float damage) {
            if (destroyable)
                CurrentHP -= damage;
            if (CurrentHP <= 0) 
                Destroy();
            else
                transform.localScale = Vector3.one * (CurrentHP / initialHP) * InitialScaling;
        }

        protected virtual void Update() {
            if (IsBurning)
                CurrentHP -= Time.deltaTime * 2;

            if (CurrentHP <= 0)
                Destroy();
        }
    }
}
