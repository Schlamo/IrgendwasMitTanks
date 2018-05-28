﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    private const float maxHealth = 10.0f;
    private float health = 10.0f;
    
    private bool isBurning = false;
	
	void Update ()
    {
		if(isBurning)
        {
            this.transform.Find("BurningParticles").gameObject.GetComponent<ParticleSystem>().Emit(1);
            health -= Time.deltaTime*5;
        }

        float scale = (health + 10) / (maxHealth + 10);
        transform.localScale = new Vector3(scale, scale, scale);
        if (health < 0.0f)
        {
            ProjectileManager.instance.createExplosion(transform.position, 2);
            GameManager.instance.DeleteTree(this);
            Destroy(this.gameObject);
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.DeleteTree(this);
            Destroy(this.gameObject);
        }
    }

    public bool IsBurning
    {
        get
        {
            return isBurning;
        }

        set
        {
            isBurning = value;
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
