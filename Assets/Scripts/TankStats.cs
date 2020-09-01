using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStats {
    public int playerId;
    public int kills;
    public int deaths;

    /**
     * Contains the id of the last player who dealt damage to this tank
     * 
     * -1 when no player dealt any damage in a certain time (defined in lastDamageTaken)
     * or whenever the player respawns
     */
    public int lastDamage;

    /**
     * Used to not display multiple explosions
     * 
     * true when the player gets killed
     * false when the player respawns
     */ 
    public bool died;

    /** The time it takes the tank to explode after it was destroyed (seconds) */
    public float timeToExplode;

    /** How much the use of nitro amplifies the movement speed of the tank */
    public float nitroAmp;

    /** Disables tanks movements as long as there is no ground contact */
    public bool canDrive;

    /** Defines whether the tank can shoot or not  */
    public float canShoot;

    /** The time it takes to reload (seconds) */
    public float reloadTime;

    /** Accumulated amount of nitro (seconds) */
    public float nitro;

    /** How long updates will last (seconds) */
    public float powerUpDuration;

    /** The rotation speed of the tank */
    public float rotationSpeed;

    public float defaultSpeed;
    public float currentSpeed;
    public float speedGained;
    public float maxSpeed;
    public float speed;

    public float defaultDamage;
    public float damageGained;
    public float maxDamage;
    public float damage;

    public float defaultArmor;
    public float armorGained;
    public float maxArmor;
    public float armor;

    public float defaultHealth;
    public float maxHealth;
    public float health;

}
