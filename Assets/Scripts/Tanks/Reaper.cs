﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reaper : Tank
{
    public float specialCooldown = 15.0f;
    public float specialDuration = 3.0f;

    public float specialTimer = 0.0f;
    public float currentSpecialTime = 0.0f;

    private bool specialActive = false;
    
    public override void UpdateSpecialStats()
    {
        var canvas = transform.Find("Canvas");
        if (specialTimer < specialCooldown)
        {
            canvas.Find("Special").GetComponent<Text>().text = (specialCooldown - specialTimer).ToString("F1") + "s";
        }
        else
        {
            canvas.Find("Special").GetComponent<Text>().text = "Ready";
        }
    }

    public override void UpdateSpecial(float dTime, GamePad.Index idx)
    {
        if(!specialActive && specialTimer < specialCooldown)
        {
            specialTimer += dTime;
        }
        else if(specialActive)
        {
            transform.Find("ImmunityParticles").GetComponent<ParticleSystem>().Emit((int)(250.0f*dTime));
            currentSpecialTime += dTime;

            if (currentSpecialTime >= specialDuration)
            {
                currentSpecialTime = 0.0f;
                specialActive = false;
                armor = defaultArmor;
            }
        }
    }

    public override void Special()
    {
        if(specialTimer >= specialCooldown)
        {
            specialTimer = 0.0f;
            specialActive = true;
            armor = maxArmor;
        }
    }
}