using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public Transform wheel;
    public float animationDuration = 2.0f;
    


    private bool spinRight = false;
    private bool spinLeft = false;
    private float rotationTarget;
    private float rotationToGo;

    private int currentWheel = 1;
	// Use this for initialization
	void Start () {
        try
        {
            wheel = GameObject.Find("MainWheel").transform;
        }
        catch(System.Exception)
        {
            Application.Quit();
        }

    }
	
	// Update is called once per frame
	void Update () {
        float xAxis = 0.0f;
        xAxis += GamePad.GetAxis(GamePad.Axis.Dpad, GamePad.Index.Any).x;
        xAxis += GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x;
        xAxis += GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any).x;

        if(!spinLeft && !spinRight)
        { 
            if (xAxis >= 0.75)
            {
                SpinRight();
            }
            else if(xAxis <= -0.75)
            {
                SpinLeft();
            }
        }
        else if(spinRight)
        {
            Vector3 rot = new Vector3(0, 0, 0);
            switch (wheel.GetComponent<Wheel>().axis)
            {
                case 0:
                    rot = new Vector3(-animationDuration, 0, 0);
                    break;
                case 1:
                    rot = new Vector3(0, -animationDuration, 0);
                    break;
                case 2:
                    rot = new Vector3(0, 0, -animationDuration);
                    break;
            }

            wheel.transform.Rotate(rot);
            rotationToGo += animationDuration;

            if(rotationToGo >= rotationTarget)
            {
                spinRight = false;
                rotationToGo = 0;
            }
        }
        else if (spinLeft)
        {
            Vector3 rot = new Vector3(0,0,0);
            switch(wheel.GetComponent<Wheel>().axis)
            {
                case 0:
                    rot = new Vector3(animationDuration, 0, 0);
                    break;
                case 1:
                    rot = new Vector3(0, animationDuration, 0);
                    break;
                case 2:
                    rot = new Vector3(0, 0, animationDuration);
                    break;
            }

            wheel.transform.Rotate(rot);
            rotationToGo += animationDuration;

            if (rotationToGo >= rotationTarget)
            {
                spinLeft = false;
                rotationToGo = 0;
            }
        }
    }

    private void ChangeWheel(string name)
    {

    }

    private void SpinLeft()
    {
        spinLeft = true;
        rotationTarget = wheel.GetComponent<Wheel>().Angle;
    }

    private void SpinRight()
    {
        spinRight = true;
        rotationTarget = wheel.GetComponent<Wheel>().Angle;
    }
}
