using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    private int owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tank")
        {
            try
            {
                Tank tank = other.gameObject.GetComponent<Tank>();
                if (tank.padNumber != owner)
                {
                    tank.TakeDamage(25);
                    Vector3 forceDirection = tank.transform.position - transform.position;
                    forceDirection.Normalize();
                    forceDirection.y = 1;

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
