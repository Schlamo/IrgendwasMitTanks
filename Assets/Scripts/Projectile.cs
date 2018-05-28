using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private int owner;
    private float damage = 0.0f;
    private int type = 0;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(this.transform.position.y < -5)
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
                ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
                var crate = other.gameObject.GetComponent<Crate>();
                crate.Hit(damage);
            }
            else if (other.gameObject.tag == "Tree")
            {
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
                ProjectileManager.instance.createExplosion(pos, 1);
            }
            else if (collision.gameObject.tag == "Tank")
            {
                ProjectileManager.instance.createExplosion(pos, 0);
                var tank = collision.gameObject.GetComponent<Tank>();
                tank.TakeDamage(this.damage);
                if(Mathf.Round(tank.health) <= 0)
                {
                    tank.Explode();
                    GameManager.instance.GiveKillToPlayer(this.owner);
                }
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
