using UnityEngine;

namespace PowerUps {
    public class AnimatedPowerUp : PowerUp {
        public Transform rotationAxisI;
        public Transform rotationAxisII;
        public Transform light;
        public  float initialRotationSpeed = 15.0f;
        private float currentRotationSpeed = 15.0f;
        private float initialIntensity = 2.5f;

        public void Update() {
            base.Update();
            light.transform.GetComponent<Light>().intensity = initialIntensity * (timer / lifetime);
            currentRotationSpeed = initialRotationSpeed * (lifetime / timer);

            transform.Rotate     (new Vector3(0.0f, 90.0f * Time.deltaTime, 0.0f));
            rotationAxisI.Rotate (new Vector3(0.0f, currentRotationSpeed * 5.0f * Time.deltaTime, 0.0f));
            rotationAxisII.Rotate(new Vector3(0.0f, currentRotationSpeed * 5.0f * Time.deltaTime, 0.0f));
        }
    }
}
