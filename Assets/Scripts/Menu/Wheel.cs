using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float angle = 0.0f;
    public int id;    

    /* X:0
     * Y:1
     * Z:2
     */
    public int axis;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
        }
    }
}
