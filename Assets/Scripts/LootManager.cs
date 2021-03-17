using Enumerators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    private static Transform SpeedUp  { get; set;}
    private static Transform DamageUp { get; set;}
    private static Transform ArmorUp  { get; set;}
    private static Transform RepairUp { get; set;}
    private static Transform Mine     { get; set;}

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DamageUp = (Resources.Load("PowerUp/DamageUp")  as GameObject).transform;
        RepairUp = (Resources.Load("PowerUp/RepairKit") as GameObject).transform;
        SpeedUp  = (Resources.Load("PowerUp/SpeedUp")   as GameObject).transform;
        ArmorUp  = (Resources.Load("PowerUp/ArmorUp")   as GameObject).transform;
        Mine     = (Resources.Load("Obstacles/Mine")    as GameObject).transform;
    }

    public static LootManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public static void CreateLoot(Transform t, float health)
    {
        int rnd = Random.Range(1, 101);
        if(rnd < 10)
        {
            var mine = Instantiate(Mine, t.position, t.rotation);
            mine.transform.GetComponent<Mine>().Owner = PlayerIndex.Default;
            mine.transform.Translate(new Vector3(0, -1.5f, 0));
            return;
        }

        if (health < 10.0f)
            Instantiate(RepairUp, t.position, t.rotation);

        else if(health <= 50.0f)
        {
            rnd = Random.Range(0, 2);
            if(rnd == 1)
                Instantiate(RepairUp, t.position, t.rotation);
            else
            {
                rnd = Random.Range(1, 4);

                switch (rnd)
                {
                    case 1:
                        Instantiate(SpeedUp, t.position, t.rotation);
                        break;
                    case 2:
                        Instantiate(DamageUp, t.position, t.rotation);
                        break;
                    case 3:
                        Instantiate(ArmorUp, t.position, t.rotation);
                        break;
                }
            }
        }

        //If HP is >50%, the powerUp is 1/3 a speedUp/damageUp/armorUp
        else
        {
            rnd = Random.Range(1, 4);
            switch (rnd)
            {
                case 1:
                    Instantiate(SpeedUp, t.position, t.rotation);
                    break;
                case 2:
                    Instantiate(DamageUp, t.position, t.rotation);
                    break;
                case 3:
                    Instantiate(ArmorUp, t.position, t.rotation);
                    break;
            }
        }
    }
}
