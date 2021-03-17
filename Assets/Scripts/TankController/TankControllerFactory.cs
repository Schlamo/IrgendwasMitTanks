using Enumerators;

namespace Controllers
{
    public static class TankControllerFactory
    {
        public static ITankController GetTankController(PlayerConfiguration configuration)
        {
            // Todo: ValidationCheck?
            return configuration.Controller == ControllerType.Keyboard
                ? new KeyboardController(configuration.PlayerIndex) as ITankController
                : new GamepadController(configuration.PlayerIndex) as ITankController;
        }
    }
}
