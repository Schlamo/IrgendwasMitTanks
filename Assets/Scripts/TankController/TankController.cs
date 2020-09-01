using UnityEngine;
using UnityEditor;

public class TankController 
{
    private void Update()
    {

    }

    public TankController(int playerId, bool useController)
    {
        /** Since only two players may play with the keyboard simultaneously, 
         *  the Initialization only takes place when the playerId is <3
         *  Bullshit. Should happen in the GameManager
         */
        if (!(playerId > 2))
        {

        }


    }
}