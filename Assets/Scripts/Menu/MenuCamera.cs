using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuCamera : MonoBehaviour
    {
        public void Update() => transform.Rotate(0.0f, 0.01f, 0.0f);
    }
}
