﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Cthulu : Tank
{

    public float specialCooldown = 15.0f;
    public float specialDuration = 3.0f;
    private float healthWhenUsed = 0.0f;

    public float specialTimer = 15.0f;
    public float specialTimer2 = 0.0f;
    private bool specialActive = false;

    public override void UpdateSpecialStats()
    {
        var canvas = transform.Find("Canvas");
        if (!specialActive && specialTimer>= specialCooldown)
        {
            canvas.Find("Special").GetComponent<Text>().text = "Ready";
        } else
        {
            canvas.Find("Special").GetComponent<Text>().text = (specialCooldown - specialTimer).ToString("F1") + "s";
        }
    }

    public override void UpdateSpecial(float dTime, GamePad.Index idx)
    {
        if (specialTimer < specialCooldown && !specialActive)
        {
            specialTimer += dTime;
        }

        if(specialActive)
        {
            specialTimer2 += dTime;
            if (specialTimer2 >= specialDuration || CanShoot > 0.0f || health < healthWhenUsed)
            {
                ShowAgain();
                specialActive = false;
                specialTimer2 = 0.0f;
            }
        }
    }

    public override void Special()
    {
        healthWhenUsed = health;
        if(specialTimer >= specialCooldown)
        {
            specialTimer = 0.0f;
            specialActive = true;
            foreach(Transform child in transform)
            {
                try
                {
                    var renderer = child.GetComponent<MeshRenderer>();
                    renderer.enabled = false;
                }
                catch(System.Exception e){}
            }
        }
    }

    public void ShowAgain()
    {
        foreach (Transform child in transform)
        {
            try
            {
                var renderer = child.GetComponent<MeshRenderer>();
                renderer.enabled = true;
            }
            catch (System.Exception e)
            {

            }
        }
    }
}