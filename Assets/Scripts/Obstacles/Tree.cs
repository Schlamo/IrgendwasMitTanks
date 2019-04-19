using UnityEngine;

namespace Obstacles {
    public class Tree : Obstacle {
        public void Update() {
            base.Update();

            float scaling = (currentHP/initialHP) * initialScaling;

            transform.localScale = new Vector3(scaling, scaling, scaling);
        }

        protected override void Destroy() {
            Destroy(gameObject);
        }
    }
}
