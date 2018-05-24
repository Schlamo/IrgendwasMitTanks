using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : Tank
{

    private bool specialReady = true;
    private float specialCooldown = 30.0f;
    private float specialTimer = 0.0f;

    public override void UpdateSpecial(float dTime)
    {
        if(!specialReady)
        {
            specialTimer += dTime;
            if(specialTimer >= specialCooldown)
            {
                specialReady = true;
                specialTimer = 0.0f;
            }
        }
    }

    public override void Special()
    {
        if(specialReady)
        {
            specialReady = false;
            Transform t = launchPosition;
            float launchY = launchPosition.position.y;
            t.position = new Vector3(t.position.x, 0, t.position.z);

            for(int i = 0; i < 12; i++)
            {
                t.RotateAround(this.transform.position, Vector3.up, 30);
                t.position = new Vector3(t.position.x, launchY, t.position.z);
                Shoot(t);
            }
        }
    }
}