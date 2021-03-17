using UnityEngine;

namespace Menu
{
    public class MenuTank : MonoBehaviour
    {
        public float a;
        public float x;
        public float y;
        public float z;
        void Update() 
            => transform.RotateAround(Vector3.zero, new Vector3(x, y, z), a);
    }
}
