using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    public Transform speedUp;
    public Transform damageUp;
    public Transform armorUp;
    public Transform repairUp;

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

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static LootManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public void createLoot(Transform t)
    {
        int rnd = Random.Range(1,5);
        /* 1: Speed
         * 2: Damage
         * 3: Armor
         * 4: Repair
         * 5: Nitro
         */
        switch(rnd)
        {
            case 1:
                Instantiate(speedUp, t.position, t.rotation);
                break;
            case 2:
                Instantiate(damageUp, t.position, t.rotation);
                break;
            case 3:
                Instantiate(armorUp, t.position, t.rotation);
                break;
            case 4:
                Instantiate(repairUp, t.position, t.rotation);
                break;
        }
    }
}
