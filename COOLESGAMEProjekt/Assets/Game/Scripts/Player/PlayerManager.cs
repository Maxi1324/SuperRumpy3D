using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInfo PInfo;
    public SimplePlayerMovement SPMovement;
    public int AllowedMoves { get; set; } = 0;

    private void Reset()
    {
        PInfo = GetComponent<PlayerInfo>();
        SPMovement = GetComponent<SimplePlayerMovement>();
    }

    private void Update()
    {
        string added = (PInfo.PlayerNum == 0) ? "" : PInfo.PlayerNum.ToString();

        float XAxis = Input.GetAxis(PInfo.XAxis + added);
        float YAxis = Input.GetAxis(PInfo.YAxis + added);
        bool run = Input.GetButton(PInfo.B + added);
        bool jump = Input.GetButtonDown(PInfo.A + added);

        if (jump && AllowedMoves == 0) SPMovement.StartJump();
        if(AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, run);
        
        //if(maxSpeed != -1)MaxSpeed();
    }

    private void MaxSpeed()
    {
        Rigidbody rigidbody = PInfo.Rb;
        if (rigidbody.velocity.magnitude > PInfo.MaxSpeed)
        {
            Vector3 newV = rigidbody.velocity.normalized * PInfo.MaxSpeed;
            rigidbody.velocity = new Vector3(newV.x, rigidbody.velocity.y, newV.z);
        }
    }
}
