using UnityEngine;
namespace Obstacles {
    public class Crate : Obstacle {
        protected override void Destroy() {
            transform.Translate(new Vector3(0, 1, 0));
            LootManager.CreateLoot(transform, 10.0f);
            Destroy(gameObject);
        }

        public void Update() {
            float scaling = Mathf.Max((CurrentHP) / (initialHP) * InitialScaling, 1.0f);

            base.Update();

            transform.localScale = new Vector3(scaling, InitialScaling, scaling);
            base.Update();
        }
    }
}
