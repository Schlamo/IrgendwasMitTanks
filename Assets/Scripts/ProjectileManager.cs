using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0618 // variable declared but not used.

public class ProjectileManager : MonoBehaviour {
    public static ProjectileManager instance = null;

    public Transform Projectile;
    public Transform FlameProjectile;

    public float projectileSpeed;
    public float flameSpeed;

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
        AudioManager.instance.PlayShootSound();
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

        Vector3 direction = lPos - tPos;

        rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }

    public void createFlameProjectile(Transform tank, Transform launch, float damage, int owner)
    {
        var projectile = Instantiate(FlameProjectile, launch.position, launch.rotation);

        var pScript = projectile.GetComponent<Projectile>();

        pScript.Damage = damage;
        pScript.Owner = owner;
        pScript.Type = 1;

        var rb = projectile.GetComponent<Rigidbody>();
        projectile.transform.rotation = launch.rotation;
        projectile.transform.Rotate(new Vector3(90, 0, 0));

        Vector3 tPos = tank.position;
        Vector3 lPos = launch.position;

        tPos.y = 0;
        lPos.y = 0;

        Vector3 direction = lPos - tPos;

        rb.AddForce(direction.normalized * flameSpeed, ForceMode.Impulse);
    }

    public void createExplosion(Vector3 pos, int type = 0)
    {
        string path = "Explosions/";
        if (type == 1)
        {
            //Mud
            switch (GameManager.instance.mapType)
            {
                case 0:
                    path += "MudExplosion";
                    break;
                case 1:
                    path += "SandExplosion";
                    break;
                case 2:
                    path += "SnowExplosion";
                    break;
            }
        }
        else if(type == 2)
        {
            path += "WoodExplosion";
        }
        else if( type == 3)
        {
            path += "TankExplosion";
        }
        else
        {
            path += "Explosion";
        }

        GameObject explosion = Instantiate(Resources.Load(path), pos, new Quaternion(0, 0, 0, 0)) as GameObject;
        Destroy(explosion, 0.5f);
    }
}
