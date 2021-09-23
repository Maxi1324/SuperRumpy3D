using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInfo PInfo;
    public SimplePlayerMovement SPMovement;
    public float maxSpeed = 1f;

    private void Reset()
    {
        PInfo = GetComponent<PlayerInfo>();
        SPMovement = GetComponent<SimplePlayerMovement>();
    }

    private void Update()
    {
        float XAxis = Input.GetAxis(PInfo.XAxis);
        float YAxis = Input.GetAxis(PInfo.YAxis);
        bool run = Input.GetButtonDown(PInfo.B);

        SPMovement.SimpleMovement(XAxis, YAxis, run);
        MaxSpeed();
    }

    private void MaxSpeed()
    {
        Rigidbody rigidbody = PInfo.rb;
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }

    }
}
