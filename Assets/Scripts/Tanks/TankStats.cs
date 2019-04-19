namespace Tanks {
    public class TankStats {
        public int PlayerId { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int LastDamage { get; set; }
        public bool Died { get; set; }
        public float TimeToExplode { get; set; }
        public float NitroAmp { get; set; }
        public bool CanDrive { get; set; }
        public float CanShoot { get; set; }
        public float ReloadTime { get; set; }
        public float Nitro { get; set; }
        public float PowerUpDuration { get; set; }
        public float RotationSpeed { get; set; }

        public float DefaultSpeed { get; set; }
        public float SpeedGained { get; set; }
        public float MaxSpeed { get; set; }
        public float Speed { get; set; }

        public float DefaultDamage { get; set; }
        public float DamageGained { get; set; }
        public float MaxDamage { get; set; }
        public float Damage { get; set; }

        public float DefaultArmor { get; set; }
        public float ArmorGained { get; set; }
        public float MaxArmor { get; set; }
        public float Armor { get; set; }

        public float DefaultHealth { get; set; }
        public float MaxHealth { get; set; }
        public float Health { get; set; }
    }
}
