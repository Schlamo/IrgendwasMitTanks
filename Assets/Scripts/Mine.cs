﻿using UnityEngine;

using Tanks;
public class Mine : MonoBehaviour
{

    private int owner;
    private float lifeTime = 20.0f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tank")
        {
            try
            {
                Tank tank = other.gameObject.GetComponent<Tank>();

                if (tank.playerId != owner)
                {
                    AudioManager.instance.PlayMineDetonationSound();

                    tank.lastDamage = owner;
                    tank.TakeDamage(25);

                    other.gameObject.GetComponent<Rigidbody>().velocity*=0.5f;
                    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
                    ProjectileManager.instance.CreateExplosion(transform.position);
                    Destroy(this.gameObject);
                }
            }
            catch (System.Exception) { }
        }
    }

    public int Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }
}
