using UnityEngine;

namespace Obstacles {
    public class Tree : Obstacle {

        protected override void Destroy() {
            Destroy(gameObject);
        }

        public override void Hit (float damage)
        {
            float scaling = (currentHP / initialHP) * initialScaling;
            transform.localScale = new Vector3(scaling, scaling, scaling);
        }
    }
}
