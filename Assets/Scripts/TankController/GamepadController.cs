namespace Controllers{
    class GamepadController : ITankController {
        public GamePad.Index Index { get; set; }

        public float Acceleration() {
            return GamePad.GetTrigger(GamePad.Trigger.RightTrigger, Index) - GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, Index);
        }

        public float Rotation() {
            return GamePad.GetAxis(GamePad.Axis.LeftStick, Index).x;
        }

        public bool Shoot() {
            return false;
        }

        public bool Nitro() {
            return false;
        }

        public bool Special() => GamePad.GetButton(GamePad.Button.X, Index);

        public bool SpecialDown() => GamePad.GetButtonDown(GamePad.Button.X, Index);

        public bool SpecialUp() => GamePad.GetButtonUp(GamePad.Button.X, Index);
    }
}
