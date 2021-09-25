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

    public float Beschleunigung = 2000;
    public float WalkSpeed = 5;
    public float RunSpeed = 8;

    public Vector3 normalJumpForce = new Vector3(0,1000,2000);
    public Vector3 RunJumpForce = new Vector3(0, 1000, 4000);

    public Rigidbody Rb;
    public Transform Camera;

    public float MaxSpeed = 100;

    private void Reset()
    {
        Rb = GetComponent<Rigidbody>();
    }
}
