﻿namespace Enumerators {
    public enum ControllerType
    {
        Keyboard = 0,
        Gamepad  = 1
    }

    public enum TankColor {
        Red     = 0,
        Blue    = 1,
        Green   = 2,
        Yellow  = 3,
        Pink    = 4,
        Orange  = 5,
        Black   = 6,
        White   = 7
    }

    public enum MapType {
        Undefined   = -1,
        Meadow      = 0,
        Desert      = 1,
        Snow        = 2
    }

    public enum TankType {
        Tempest     = 0,
        Viking      = 1,
        Reaper      = 2,
        Prometheus  = 3,
        Cthulu      = 4
    }

    public enum PowerUpType {
        SpeedUp     = 0,
        DamageUp    = 1,
        ArmorUp     = 2,
        RepairKit   = 3,
        Nitro       = 4
    }

    public enum ObstacleType {
        Crate   = 0,
        Tree    = 1,
        Rock    = 2
    }

    public enum ExplosionSize {
        Small   = 0,
        Medium  = 1,
        Large   = 2
    }

    public enum ExplosionType
    {
        Default = 0,
        Floor   = 1,
        Wood    = 2,
        Tank    = 3
    }

    public enum PlayerIndex
    {
        Default = 0,
        First   = 1,
        Second  = 2,
        Third   = 3,
        Fourth  = 4
    }
}


