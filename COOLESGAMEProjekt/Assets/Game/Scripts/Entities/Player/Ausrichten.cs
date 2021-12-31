using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ausrichten : MonoBehaviour
{
    public float t = .1f;
    public Quaternion goalQuat;

    private void Update()
    {
        RaycastHit hit;
        transform.rotation = Quaternion.Lerp(transform.rotation, goalQuat, t);   
    }
}
