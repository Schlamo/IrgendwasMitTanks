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
        Vector3 pos = new Vector3(t.position.x, 0.5f, t.position.z);
        switch(rnd)
        {
            case 1:
                var speed = Instantiate(speedUp, pos, t.rotation);
                break;
            case 2:
                var damage = Instantiate(damageUp, pos, t.rotation);
                break;
            case 3:
                var armor = Instantiate(armorUp, pos, t.rotation);
                break;
            case 4:
                var repair = Instantiate(repairUp, pos, t.rotation);
                break;
        }
    }
}
