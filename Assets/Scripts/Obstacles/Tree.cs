using UnityEngine;

namespace Obstacles {
    public class Tree : Obstacle {
        protected override void Destroy() {
            Destroy(gameObject);
        }


        public void OnCollisionEnter(Collision collision) 
            => Hit(collision.gameObject.CompareTag("Tank")
                ? collision.gameObject.GetComponent<Tank>().CurrentSpeed
                : collision.gameObject.GetComponent<Projectile>().Damage);
    }
}
