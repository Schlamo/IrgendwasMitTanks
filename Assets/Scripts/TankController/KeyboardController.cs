using UnityEngine;

namespace Controllers {

    class KeyboardController : ITankController{
        public int Index { get; set; }

        public float Acceleration() {
            switch (Index) {
                case 1:  return (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.S) ? 1.0f : 0.0f); 
                case 2:  return (Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.DownArrow) ? 1.0f : 0.0f); 
                default: return 0.0f;
            }
        }

        public bool Shoot() {
            switch (Index) {
                case 1:  return (Input.GetKeyDown(KeyCode.Space)); 
                case 2:  return (Input.GetKeyDown(KeyCode.Keypad0)); 
                default: return false;
            }
        }

        public bool Nitro() {
            switch (Index) {
                case 1:  return (Input.GetKey(KeyCode.LeftShift)); 
                case 2:  return (Input.GetKey(KeyCode.RightControl)); 
                default: return false;
            }
        }

        public bool Special() {
            switch (Index) {
                case 1:  return (Input.GetKey(KeyCode.F));
                default: return false;
            }
        }

        public bool SpecialDown() {
            switch (Index) {
                case 1:  return (Input.GetKeyDown(KeyCode.F));
                default: return false;
            }
        }

        public float Rotation() {
            switch (Index) {
                case 1: return (Input.GetKey(KeyCode.A) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f); 
                case 2: return (Input.GetKey(KeyCode.LeftArrow) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f); 
                default: return 0.0f;
            }
        }
    }
}
