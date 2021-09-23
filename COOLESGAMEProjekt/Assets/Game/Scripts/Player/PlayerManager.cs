using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInfo PInfo;
    public SimplePlayerMovement SPMovement;
    public float maxSpeed {get; set; } = 1f;
    public int AllowedMoves { get; set; } = 0;
    public LayerMask mask;

    private void Reset()
    {
        PInfo = GetComponent<PlayerInfo>();
        SPMovement = GetComponent<SimplePlayerMovement>();
    }

    private void Update()
    {
        float XAxis = Input.GetAxis(PInfo.XAxis);
        float YAxis = Input.GetAxis(PInfo.YAxis);
        bool run = Input.GetButton(PInfo.B);
        bool jump = Input.GetButtonDown(PInfo.A);

        if (jump && AllowedMoves == 0) SPMovement.StartJump();
        if(AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, run);
        
        if(maxSpeed != -1)MaxSpeed();
    }

    private void MaxSpeed()
    {
        Rigidbody rigidbody = PInfo.rb;
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            Vector3 newV = rigidbody.velocity.normalized * maxSpeed;
            rigidbody.velocity = new Vector3(newV.x, rigidbody.velocity.y, newV.z);
        }
    }
}
