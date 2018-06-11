﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    public Transform speedUp;
    public Transform damageUp;
    public Transform armorUp;
    public Transform repairUp;
    public Transform mine;

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

    public void createLoot(Transform t, float health)
    {
        int rnd = Random.Range(1, 100);
        if(rnd < 10)
        {
            var m = Instantiate(mine, t.position, t.rotation);
            m.transform.GetComponent<Mine>().Owner = -1;
            m.transform.Translate(new Vector3(0, -1.5f, 0));
            return;
        }

        //If HP is <10%, the powerUp is guaranteed to be a Repairkit
        if (health < 10.0f)
        {
            Instantiate(repairUp, t.position, t.rotation);
        }

        //If HP is <=50%, the powerUp ist 1/2 a Repairkit and 1/6 another powerUp  
        else if(health <= 50.0f)
        {
            rnd = Random.Range(1, 100);
            if(rnd > 50)
            {
                Instantiate(repairUp, t.position, t.rotation);
            }
            else
            {
                rnd = Random.Range(1, 4);

                switch (rnd)
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
                }
            }
        }

        //If HP is >50%, the powerUp is 1/3 a speedUp/damageUp/armorUp
        else
        {
            rnd = Random.Range(1, 4);
            Debug.Log(rnd);
            switch (rnd)
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
            }
        }
    }
}
