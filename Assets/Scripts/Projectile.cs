using UnityEngine;

using Obstacles;
using Tanks;
using Enumerators;

public class Projectile : MonoBehaviour {

    public PlayerIndex Owner { get; set; }
    public int Type { get; set; } = 0;

    public float Damage { get; set; }

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
            tank.TakeDamage(Damage);
            tank.LastDamage = Owner;
        }

        Destroy(gameObject);
    }
}
