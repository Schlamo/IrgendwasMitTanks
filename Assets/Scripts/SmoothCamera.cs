using UnityEngine;

public class SmoothCamera : MonoBehaviour {

    public float delay = 0.05F;
    public GameObject tank;

    private Vector3 offset;         

    // Use this for initialization
    void Start() {
        offset = transform.position - tank.transform.position;
    }

    void LateUpdate() {
        transform.position = tank.transform.position + offset;
        transform.rotation = Quaternion.Slerp(transform.rotation, tank.transform.rotation, Time.deltaTime*3);
    }
}