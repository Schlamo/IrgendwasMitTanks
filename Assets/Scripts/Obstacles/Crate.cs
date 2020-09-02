using UnityEngine;
namespace Obstacles {
    public class Crate : Obstacle {
        protected override void Destroy() {
            transform.Translate(new Vector3(0, 1, 0));
            LootManager.CreateLoot(transform, 10.0f);
            Destroy(this.gameObject);
        }

        public void Update() {
            float ratio = (currentHP) / (initialHP);
            float scaling = Mathf.Max(ratio * initialScaling, 1.0f);

            base.Update();

            transform.localScale = new Vector3(scaling, initialScaling, scaling);
        }
    }
}
