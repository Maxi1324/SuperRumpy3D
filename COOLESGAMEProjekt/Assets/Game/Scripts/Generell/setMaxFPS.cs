using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generell
{
    public class setMaxFPS : MonoBehaviour
    {
        public int TargetFrameRate = 60;
        void Start()
        {
            Application.targetFrameRate = TargetFrameRate;
        }
    }
}