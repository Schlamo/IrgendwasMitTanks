namespace Controllers {
    public interface ITankController {
        float Acceleration();

        float Rotation();

        bool Shoot();

        bool Nitro();

        bool Special();

        bool SpecialDown();
    }
}