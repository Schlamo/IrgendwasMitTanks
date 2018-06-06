using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float bonus = 12.5f;
    public int type;
    private float lifeTime = 0.0f;
    public float maxLifeTime = 0.0f;
    /* 1: Speed
     * 2: Damage
     * 3: Armor
     * 4: Repair
     * 5: Nitro
     */

    public float rotationSpeed = 100.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime > 10.0f && maxLifeTime == 0.0f)
        {
            Destroy(this.gameObject);
        } else if(lifeTime > maxLifeTime && lifeTime > 0.0f)
        {
            if(this.type == 5)
            {
                GameManager.instance.DeleteNitro(this);
            }
            Destroy(this.gameObject);
        }
        this.transform.Rotate(new Vector3(0.0f, rotationSpeed * Time.deltaTime, 0.0f));
    }
}
