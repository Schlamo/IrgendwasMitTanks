using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Prometheus : Tank
{
    public float specialAccumulated = 1.0f;

    public float specialMax = 3.0f;

    public float flameDamage = 10.0f;

    private int framesToFlame = 5;
    private int specialFrames = 0;

    private float delta = 0.0f;

    public override void UpdateSpecial(float dTime, GamePad.Index idx)
    {
        delta = dTime;
        specialAccumulated = specialAccumulated > specialMax ? specialMax : specialAccumulated + (delta / 10);

        if (GamePad.GetButton(GamePad.Button.B, idx))
        {
            OnButtonSpecial();
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
                specialAccumulated -= delta * framesToFlame;
                ProjectileManager.instance.createFlameProjectile(this.gameObject.transform, this.launchPosition, flameDamage * delta * framesToFlame, this.padNumber);
                specialFrames = framesToFlame;
            }
        }
    }
}