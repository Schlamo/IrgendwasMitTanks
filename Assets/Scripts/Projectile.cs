using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private int owner;
    private float damage;
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
        if (other.gameObject.tag == "Crate")
        {
            ProjectileManager.instance.createExplosion(other.gameObject.transform.position, 2);
            var crate = other.gameObject.GetComponent<Crate>();
            crate.Hit(damage);
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = this.gameObject.transform.position;

        if (collision.gameObject.tag == "Map")
        {
            ProjectileManager.instance.createExplosion(pos, 1);
        }else if(collision.gameObject.tag == "Tree")
        {
            ProjectileManager.instance.createExplosion(pos, 2);
        }
        else if(collision.gameObject.tag == "Tank")
        {
            ProjectileManager.instance.createExplosion(pos, 0);
            try
            {
                var tank = collision.gameObject.GetComponent<Tank>();
                tank.TakeDamage(this.damage);
            }
            catch (System.Exception e) { Debug.LogError(e); }
        }
        else
        {
            ProjectileManager.instance.createExplosion(pos, 0);
        }
        Destroy(this.gameObject);
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
