using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningTank : MonoBehaviour {
    
	void Start () {
		
	}

    public void Paint(int color)
    {
        Material mat;
        string path = "Colors/";

        switch (color)
        {
            case 0:
                path += "red";
                break;
            case 1:
                path += "blue";
                break;
            case 2:
                path += "green";
                break;
            case 3:
                path += "yellow";
                break;
            case 4:
                path += "pink";
                break;
            case 5:
                path += "orange";
                break;
            case 6:
                path += "black";
                break;
            case 7:
                path += "white";
                break;
        }
        mat = Resources.Load(path) as Material;

        foreach (Transform child in transform)
        {
            if (child.tag == "Colorized")
            {
                child.GetComponent<Renderer>().material = mat;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
