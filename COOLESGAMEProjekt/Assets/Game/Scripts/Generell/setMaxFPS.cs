using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setMaxFPS : MonoBehaviour
{
    public int TargetFrameRate = 60;
    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
