/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public Text red_HP;
    public Text blue_HP;
    void Start () {
	}
	
	void Update () {

    }

    public static void UpdateHP(int player, int hp)
    {
        switch (player)
        {
            case 1:
                GameObject.Find("HP_red").GetComponent<Text>().text = "HP: " + hp;
                break;
            case 2:
                GameObject.Find("HP_blue").GetComponent<Text>().text = "HP: " + hp;
                break;
        }
    }

    public static void UpdateKills(int player, int kills)
    {
        switch (player)
        {
            case 1:
                GameObject.Find("Kills_red").GetComponent<Text>().text = "Kills: " + kills;
                break;
            case 2:
                GameObject.Find("Kills_red").GetComponent<Text>().text = "Kills: " + kills;
                break;
        }
    }
}
*/