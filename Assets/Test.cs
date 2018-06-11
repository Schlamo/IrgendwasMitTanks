using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (GameObject.Find("Center").transform.position - transform.position).normalized;
        var rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(direction * 10000 * Time.deltaTime);
    }
}
