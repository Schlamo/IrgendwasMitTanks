using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Viking : Tank
{

    private bool specialReady = true;
    private float specialCooldown = 10.0f;
    private float specialTimer = 0.0f;

    public override void UpdateSpecialStats()
    {
        var canvas = transform.Find("Canvas");
        if (specialReady)
        {
            canvas.Find("Special").GetComponent<Text>().text = "Ready";
        }
        else
        {
            canvas.Find("Special").GetComponent<Text>().text = (specialCooldown - specialTimer).ToString("F1") + "s";
        }
    }

    public override void UpdateSpecial(float dTime, GamePad.Index idx)
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

            t.RotateAround(this.transform.position, Vector3.up, -20);
            for (int i = 0; i < 4; i++)
            {
                t.RotateAround(this.transform.position, Vector3.up, 10);
                t.position = new Vector3(t.position.x, launchY, t.position.z);
                ProjectileManager.instance.createProjectile(this.transform, t, this.damage / 2.0f, this.padNumber);
            }
            t.RotateAround(this.transform.position, Vector3.up, 340);
        }
    }
}