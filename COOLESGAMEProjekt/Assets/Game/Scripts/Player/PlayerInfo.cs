using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int PlayerNum = 0;
    public string XAxis= "Horizontal";
    public string YAxis  = "Vertical";
    public string A  = "Fire1";
    public string B  = "Fire2";
    public string C  = "Fire3";
    public Rigidbody rb;

    public float WalkSpeed = 5;
    public float RunSpeed = 10;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }
}
