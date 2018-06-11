using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                if (tank.padNumber != owner)
                {
                    AudioManager.instance.PlayMineDetonationSound();

                    tank.LastDamage = owner;
                    tank.TakeDamage(25);

                    other.gameObject.GetComponent<Rigidbody>().velocity*=0.5f;
                    ProjectileManager.instance.createExplosion(transform.position);
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
