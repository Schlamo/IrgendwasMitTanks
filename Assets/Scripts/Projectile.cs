using UnityEngine;

using Obstacles;
using Tanks;
using Enumerators;

public class Projectile : MonoBehaviour {

    private int owner;
    private float damage = 0.0f;
    private int type = 0;

    public float Damage { get; set; }
    public int Type { get; set; }
    public int Owner { get; set; }

    // Update is called once per frame
    void Update() {
        if(transform.position.y < -15)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        var position = gameObject.transform.position;

        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Tree")) {
            try {
                Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

                if (obstacle.isDestroyable)
                    obstacle.Hit(Damage);

                ProjectileManager.instance.CreateExplosion(
                    collision.contacts[0].point,
                    new Gradient[] { obstacle.explosionGradientA, obstacle.explosionGradientB },
                    ExplosionSize.Small);
            }
            catch(System.Exception) { }
        }

        if (collision.gameObject.CompareTag("Map")) {
            AudioManager.instance.PlayMapImpactSound();
            ProjectileManager.instance.CreateExplosion(position, ExplosionType.Floor);
        }
        else if (collision.gameObject.CompareTag("Tank")) {
            AudioManager.instance.PlayTankImpactSound();
            ProjectileManager.instance.CreateExplosion(position, ExplosionType.Tank);
            var tank = collision.gameObject.GetComponent<Tank>();
            tank.TakeDamage(damage);
            tank.lastDamage = owner;
        }

        Destroy(gameObject);
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
}
