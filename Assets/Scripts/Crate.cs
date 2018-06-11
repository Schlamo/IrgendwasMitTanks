using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

    private const float maxHealth = 10.0f;
    private float health = 10.0f;

    public void Hit(float damage, float percentageHealth)
    {
        health -= damage;
        float scale = (health+10) / (maxHealth+10);
        transform.localScale = new Vector3(scale*3, 3, scale*3);
        if(health <= 0)
        {
            GameManager.instance.DeleteCrate(this);
            LootManager.Instance.createLoot(this.transform, percentageHealth);
            Destroy(this.gameObject);
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }
}
