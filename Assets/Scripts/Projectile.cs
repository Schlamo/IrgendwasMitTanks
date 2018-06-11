using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0618 // variable declared but not used.

public class Projectile : MonoBehaviour {

    private int owner;
    private float damage = 0.0f;
    private int type = 0;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(this.transform.position.y < -15)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(type == 0)
        {
            if (other.gameObject.tag == "Crate")
            {
                AudioManager.instance.PlayCrateImpactSound();
                ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
                var crate = other.gameObject.GetComponent<Crate>();
                crate.Hit(damage, GameManager.instance.GetPlayerByIdx(owner).GetPercentageHealth());
            }
            else if (other.gameObject.tag == "Tree")
            {
                AudioManager.instance.PlayCrateImpactSound();
                var tree = other.gameObject.GetComponent<Tree>();
                tree.Hit(damage/3);
                ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
            }
        }
        else
        {
            if (other.gameObject.tag == "Tree")
            {
                var tree = other.gameObject.GetComponent<Tree>();
                tree.Hit(damage);
                tree.IsBurning = true;
            }
            else if (other.gameObject.tag == "Crate")
            {
                ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
                var crate = other.gameObject.GetComponent<Crate>();
                crate.Hit(damage, GameManager.instance.GetPlayerByIdx(owner).GetPercentageHealth());
            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = this.gameObject.transform.position;

        if (type == 0)
        {
            
            if (collision.gameObject.tag == "Map")
            {
                AudioManager.instance.PlayMapImpactSound();
                ProjectileManager.instance.createExplosion(pos, 1);
            }
            else if (collision.gameObject.tag == "Tank")
            {
                AudioManager.instance.PlayTankImpactSound();
                ProjectileManager.instance.createExplosion(pos, 0);
                var tank = collision.gameObject.GetComponent<Tank>();
                tank.TakeDamage(this.damage);
                tank.LastDamage = owner;
            }
            else
            {
                ProjectileManager.instance.createExplosion(pos, 0);
            }
            Destroy(this.gameObject);
        }
        else if(type == 1)
        {
            if(collision.gameObject.tag == "Tank")
            {
                var tank = collision.gameObject.GetComponent<Tank>();
                tank.LastDamage = owner;
                tank.TakeTrueDamage(this.damage);
            }
            ProjectileManager.instance.createExplosion(pos);
            Destroy(this.gameObject.GetComponent<SphereCollider>());
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            this.gameObject.GetComponent<ParticleSystem>().Stop();
            Destroy(this.gameObject, this.gameObject.GetComponent<ParticleSystem>().startLifetime);
        }
    }

    public float Damage
    {
        get
        {
            return this.damage;
        }
        set
        {
            this.damage = value;
        }
    }

    public int Type
    {
        get
        {
            return this.type;
        }
        set
        {
            this.type = value;
        }
    }

    public int Owner
    {
        get
        {
            return this.owner;
        }
        set
        {
            this.owner = value;
        }
    }
}
