using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInfo PInfo;
    public SimplePlayerMovement SPMovement;
    public int AllowedMoves { get; set; } = 0;

    public Transform groundTrans;
    public LayerMask groundMask;

    public string added {get; set;}
    public float XAxis {get; set;}
    public float YAxis {get; set;}
    public bool run {get; set;}
    public bool jumpPressed {get; set;}
    public bool jump {get; set;}

    public bool onGround { get; set; }

    private void Reset()
    {
        PInfo = GetComponent<PlayerInfo>();
        SPMovement = GetComponent<SimplePlayerMovement>();
        Application.targetFrameRate = 240;
    }

    private void FixedUpdate()
    {
        added = (PInfo.PlayerNum == 0) ? "" : PInfo.PlayerNum.ToString();

        XAxis = Input.GetAxis(PInfo.XAxis + added);
        YAxis = Input.GetAxis(PInfo.YAxis + added);
        run = Input.GetButton(PInfo.B + added);
        jumpPressed = Input.GetButtonDown(PInfo.A + added);
        jump = Input.GetButton(PInfo.A + added);

        onGround = Physics.CheckSphere(groundTrans.position, .1f, groundMask);

        if (jumpPressed && AllowedMoves == 0 && onGround) SPMovement.StartJump();
        if(AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, run,1);
        if(AllowedMoves == 1) SPMovement.SimpleMovement(XAxis, YAxis, run,PInfo.SpeedMultInAir);
        
        MaxSpeed();
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
