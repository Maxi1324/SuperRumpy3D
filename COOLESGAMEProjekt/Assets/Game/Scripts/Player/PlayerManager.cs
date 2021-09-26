using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInfo PInfo;
    public SimplePlayerMovement SPMovement;

    public List<MovementAbility> Moves; 

    public int AllowedMoves { get; set; } = 0;

    public Transform groundTrans;
    public LayerMask groundMask;

    public string added {get; set;}
    public float XAxis {get; set;}
    public float YAxis {get; set;}
    public bool Run {get; set;}
    public bool JumpPressed {get; set;}
    public bool Jump {get; set;}
    public bool Fire3Pressed {get; set;}
    public bool Fire3Up { get; set;}
    public bool Fire3 { get; set;}

    public bool OnGround { get; set; }

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
        Run = Input.GetButton(PInfo.B + added);
        JumpPressed = Input.GetButtonDown(PInfo.A + added);
        Jump = Input.GetButton(PInfo.A + added);
        Fire3Pressed = Input.GetButtonDown(PInfo.C + added);
        Fire3 = Input.GetButton(PInfo.C + added);
        Fire3Up = Input.GetButtonUp(PInfo.C + added);

        OnGround = Physics.CheckSphere(groundTrans.position, .3f, groundMask);

        InteractPlayer[] InteractPlayers = FindObjectsOfType<InteractPlayer>();

        for (int i = 0; i < InteractPlayers.Length; i++) {
            InteractPlayer InteractPlayer = InteractPlayers[i];
            float dis = Vector3.Distance(InteractPlayer.transform.position, transform.position);
            Moves.ForEach((MovementAbility ability) =>
            {
                 ability.Active(dis, InteractPlayer, ability.Allowed(AllowedMoves));
            });
        }

        if (Jump && AllowedMoves == 0 && OnGround) SPMovement.StartJump();
        if(AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, Run,1);
        if(AllowedMoves == 1) SPMovement.SimpleMovement(XAxis, YAxis, Run,PInfo.SpeedMultInAir);
       // MaxSpeed();
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
