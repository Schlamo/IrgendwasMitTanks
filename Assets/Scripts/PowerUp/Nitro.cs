using UnityEngine;

namespace PowerUps {
    public class Nitro : PowerUp {
        private float rotationSpeed;
        public void Start() {
            base.Start();

            rotationSpeed = 333.0f;
        }
        public void Update() {
            base.Update();

            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        }
    }
}
