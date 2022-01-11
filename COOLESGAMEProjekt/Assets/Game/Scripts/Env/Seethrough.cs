using Cinemachine;
using System;
using UnityEngine;

namespace Env
{
    public class Seethrough:MonoBehaviour
    {
        private Transform Camera;
        public string str = "SeeTrough";
        private Material mat;

        private void Start()
        {
            Camera = FindObjectOfType<CinemachineBrain>().transform;
            mat = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            mat.SetVector(str, Camera.transform.position);
        }
    }
}
