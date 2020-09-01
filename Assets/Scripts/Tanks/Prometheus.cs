using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Prometheus : Tank
{
    public float specialAccumulated = 1.0f;

    public float specialMax = 3.0f;

    public float flameDamage = 10.0f;
    public float timeToMaxDamage = 2.0f;

    private int framesToFlame = 5;
    private int specialFrames = 0;
		
    private float delta = 0.0f;

    public override void UpdateSpecialStats()
    {
        var canvas = transform.Find("Canvas");
        canvas.Find("Special").GetComponent<Text>().text = Mathf.Max(0.0f, specialAccumulated).ToString("F1") + "s";
    }
		

    public override void UpdateSpecial(float dTime, GamePad.Index idx)
    {
        delta = dTime;
        specialAccumulated = specialAccumulated > specialMax ? specialMax : specialAccumulated + (delta / 10);

		if(GamePad.GetButtonUp(GamePad.Button.B, idx)) 
		{
			AudioManager.instance.StopFlameSound ();
		}
			

        if (GamePad.GetButton(GamePad.Button.B, idx))
        {
            timeToMaxDamage += dTime;
            OnButtonSpecial();
        }
        else
        {
			//AudioManager.instance.StopFlameSound ();
            timeToMaxDamage = 0.0f;
        }

        if(specialFrames > 0)
        {
            specialFrames--;
        }
    }

    public override void Special()
    {

    }

    public void OnButtonSpecial()
    {
        if (specialAccumulated > delta)
        {
            if(specialFrames == 0)
            {
                AudioManager.instance.PlayFlameSound();
                float amplifier = 1.0f;
                amplifier += Mathf.Min(timeToMaxDamage, 2.0f);
                specialAccumulated -= delta * framesToFlame;
                ProjectileManager.instance.createFlameProjectile(this.gameObject.transform, this.launchPosition, flameDamage * delta * framesToFlame * amplifier, this.padNumber);
                specialFrames = framesToFlame;
            }
        }
    }
}