namespace Controllers{
    class GamepadController : ITankController {
        public GamePad.Index Index { get; set; }

        public float Acceleration() {
            return GamePad.GetTrigger(GamePad.Trigger.RightTrigger, Index) - GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, Index);
        }

        public float Rotation() {
            return 0.0f;
        }

        public bool Shoot() {
            return false;
        }

        public bool Nitro() {
            return false;
        }

        public bool Special() {
            return false;
        }

        public bool SpecialDown() {
            return false;
        }
}
}
