using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Env
{

    //Nutzt das nicht. Nutzt lieber MovingPlattform; ist besser
    public class UpDown : MonoBehaviour
    {
        public float TimeDur;
        public float distance;
        public Vector3 Dir;

        private float Speed;
        private bool AktDir;
        private float timer;

        private void Start()
        {
            Speed = distance / TimeDur;
            Dir = Dir.normalized;
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > TimeDur)
            {
                timer = 0;
                AktDir = !AktDir;
            }
            else
            {
                transform.Translate(Dir * Speed * (AktDir ? 1 : -1) * Time.deltaTime);
            }
        }
    }
}