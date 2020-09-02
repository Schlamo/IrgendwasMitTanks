using UnityEngine;

namespace Controllers {

    class KeyboardController : ITankController{
        public KeyboardController(int index) => Index = index;
        private int Index { get; }

        public float Acceleration()
            => Index == 1
                ? (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.S) ? 1.0f : 0.0f)
                : (Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.DownArrow) ? 1.0f : 0.0f); 

        public bool Shoot()
            => Index == 1
                ? Input.GetKeyDown(KeyCode.Space)
                : Input.GetKeyDown(KeyCode.Keypad0); 

        public bool Nitro()
            => Index == 1
                ? Input.GetKey(KeyCode.LeftShift)
                : Input.GetKey(KeyCode.RightControl); 
        public bool Special() 
            => Index == 1
                ? Input.GetKey(KeyCode.F)
                : false;

        public bool SpecialDown() 
            => Index == 1
                ? Input.GetKeyDown(KeyCode.F)
                : false;

        public float Rotation()
            => Index == 1
                ? (Input.GetKey(KeyCode.A) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f)
                : (Input.GetKey(KeyCode.LeftArrow) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f); 

        public bool SpecialUp() 
            => Index == 1
                ? Input.GetKeyUp(KeyCode.F)
                : Input.GetKeyUp(KeyCode.F);
    }
}
