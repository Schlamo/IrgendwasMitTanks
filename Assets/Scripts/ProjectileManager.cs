using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public Transform Projectile;

    public Transform mudExplosion;
    public Transform woodExplosion;
    public Transform tankExplosion;
    public Transform explosion;

    public float projectileSpeed;

    public static ProjectileManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void createProjectile(Transform tank, Transform launch, float damage, int owner)
    {
        var projectile = Instantiate(Projectile, launch.position, launch.rotation);

        projectile.GetComponent<Projectile>().Damage = damage;
        projectile.GetComponent<Projectile>().Owner = owner;

        var rb = projectile.GetComponent<Rigidbody>();
        projectile.transform.rotation = launch.rotation;
        projectile.transform.Rotate(new Vector3(90, 0, 0));

        Vector3 tPos = tank.position;
        Vector3 lPos = launch.position;

        tPos.y = 0;
        lPos.y = 0;

        Vector3 direction = lPos-tPos;

        rb.AddForce(direction*projectileSpeed, ForceMode.Impulse);
    }

    public void createExplosion(Vector3 pos, int type = 0)
    {
        if(type == 1)
        {
            //Mud
            var e = Instantiate(mudExplosion, pos, new Quaternion(0, 0, 0, 0));
            var p = e.GetComponent<ParticleSystem>();
            p.Emit(2);
            Destroy(e.gameObject, p.duration + p.startLifetime);
        }
        else if(type == 2)
        {
            //Wood
            var e = Instantiate(woodExplosion, pos, new Quaternion(0, 0, 0, 0));
            var p = e.GetComponent<ParticleSystem>();
            p.Emit(2);
            Destroy(e.gameObject, p.duration + p.startLifetime);
        }
        else if( type == 3)
        {
            //Tank
            var e = Instantiate(tankExplosion, pos, new Quaternion(0, 0, 0, 0));
            var p = e.GetComponent<ParticleSystem>();
            p.Emit(2);
            Destroy(e.gameObject, p.duration + p.startLifetime);
        }
        else
        {
            //Default
            var e = Instantiate(explosion, pos, new Quaternion(0, 0, 0, 0));
            var p = e.GetComponent<ParticleSystem>();
            p.Emit(2);
            Destroy(e.gameObject, p.duration + p.startLifetime);
        }
    }
}
