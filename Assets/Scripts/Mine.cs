using UnityEngine;

using Enumerators;

public class Mine : MonoBehaviour
{
    public PlayerIndex Owner { get; set; }
    private float lifeTime = 20.0f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Tank"))
        {
            try
            {
                Tank tank = collider.gameObject.GetComponent<Tank>();

                if (tank.PlayerIndex != Owner)
                {
                    AudioManager.instance.PlayMineDetonationSound();

                    tank.LastDamage = Owner;
                    tank.TakeDamage(25);

                    collider.gameObject.GetComponent<Rigidbody>().velocity*=0.5f;
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
                    ProjectileManager.instance.CreateExplosion(transform.position);
                    Destroy(this.gameObject);
                }
            }
            catch (System.Exception) { }
        }
    }
}
