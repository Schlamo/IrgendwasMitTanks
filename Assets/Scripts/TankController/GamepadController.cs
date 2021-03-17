using Enumerators;

namespace Controllers{
    class GamepadController : ITankController {
        public GamepadController(PlayerIndex index) 
            => Index = (GamePad.Index)index;
        public GamePad.Index Index { get; set; }

        public float Acceleration() {
            return GamePad.GetTrigger(GamePad.Trigger.RightTrigger, Index) - GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, Index);
        }

        public float Rotation() {
            return GamePad.GetAxis(GamePad.Axis.LeftStick, Index).x;
        }

        public bool Shoot() => GamePad.GetButtonDown(GamePad.Button.A, Index);

        public bool Nitro() => GamePad.GetButton(GamePad.Button.Y, Index);

        public bool Special() => GamePad.GetButton(GamePad.Button.X, Index);

        public bool SpecialDown() => GamePad.GetButtonDown(GamePad.Button.X, Index);

        public bool SpecialUp() => GamePad.GetButtonUp(GamePad.Button.X, Index);
    }
}
