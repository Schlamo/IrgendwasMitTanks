using UnityEngine;

using Obstacles;
using Tanks;
using Enumerators;

public class Projectile : MonoBehaviour {

    private int owner;
    private float damage = 0.0f;
    private int type = 0;

    // Update is called once per frame
    void Update() {
        if(transform.position.y < -15)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Vector3 pos = gameObject.transform.position;

        if(collision.gameObject.tag == "Obstacle") {
            try {
                Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

                if (obstacle.isDestroyable)
                    obstacle.Hit(damage);

                ProjectileManager.instance.CreateExplosion(
                    collision.contacts[0].point,
                    new Gradient[] { obstacle.explosionGradientA, obstacle.explosionGradientB },
                    ExplosionSize.Small);
            }
            catch(System.Exception) { }
        }

        if (collision.gameObject.tag == "Map") {
            AudioManager.instance.PlayMapImpactSound();
            ProjectileManager.instance.createExplosion(pos, 1);
        }
        else if (collision.gameObject.tag == "Tank") {
            AudioManager.instance.PlayTankImpactSound();
            ProjectileManager.instance.createExplosion(pos, 0);
            var tank = collision.gameObject.GetComponent<Tank>();
            tank.TakeDamage(this.damage);
            tank.lastDamage = owner;
        }
        Destroy(this.gameObject);
        /*else if(type == 1)
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
        }*/
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
