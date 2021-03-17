using Enumerators;
using UnityEngine;

namespace Controllers {

    class KeyboardController : ITankController{
        public KeyboardController(PlayerIndex index) => Index = index;
        private PlayerIndex Index { get; }

        public float Acceleration()
            => Index == PlayerIndex.First
                ? (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.S) ? 1.0f : 0.0f)
                : (Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.DownArrow) ? 1.0f : 0.0f); 

        public bool Shoot()
            => Index == PlayerIndex.First
                ? Input.GetKeyDown(KeyCode.Space)
                : Input.GetKeyDown(KeyCode.Keypad0); 

        public bool Nitro()
            => Index == PlayerIndex.First
                ? Input.GetKey(KeyCode.LeftShift)
                : Input.GetKey(KeyCode.RightControl); 
        public bool Special() 
            => Index == PlayerIndex.First
                ? Input.GetKey(KeyCode.F)
                : false;

        public bool SpecialDown() 
            => Index == PlayerIndex.First
                ? Input.GetKeyDown(KeyCode.F)
                : Input.GetKeyDown(KeyCode.RightShift);

        public float Rotation()
            => Index == PlayerIndex.First
                ? (Input.GetKey(KeyCode.A) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f)
                : (Input.GetKey(KeyCode.LeftArrow) ? -1.0f : 0.0f) + (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f); 

        public bool SpecialUp() 
            => Index == PlayerIndex.First
                ? Input.GetKeyUp(KeyCode.F)
                : Input.GetKeyUp(KeyCode.F);
    }
}
