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
    public float MaxGroundDis = 1.5f;

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
    }

    private void Update()
    {
        float y = PInfo.Rb.velocity.y;
        
        added = (PInfo.PlayerNum == 0) ? "" : PInfo.PlayerNum.ToString();
        XAxis = Input.GetAxis(PInfo.XAxis + added);
        YAxis = Input.GetAxis(PInfo.YAxis + added);
        Run = Input.GetButton(PInfo.B + added);
        JumpPressed = Input.GetButtonDown(PInfo.A + added);
        Jump = Input.GetButton(PInfo.A + added);
        Fire3Pressed = Input.GetButtonDown(PInfo.C + added);
        Fire3 = Input.GetButton(PInfo.C + added);
        Fire3Up = Input.GetButtonUp(PInfo.C + added);

        OnGround = checkGround(MaxGroundDis);
        float abnahme = 0.97f;
        if(XAxis == 0 && YAxis == 0)
        {
            abnahme = 0.94f;
        }
        PInfo.Rb.velocity = new Vector3(PInfo.Rb.velocity.x * abnahme, (y < 0f && y > -100) ? y * 1.03f : y, PInfo.Rb.velocity.z * abnahme);
        FrameAnim();

        InteractPlayer[] InteractPlayersA = FindObjectsOfType<InteractPlayer>();
        List<InteractPlayer> InteractPlayers = new List<InteractPlayer>(InteractPlayersA);
        InteractPlayers.Sort(new comp());

        for (int i = 0; i < InteractPlayers.Count; i++) {
            InteractPlayer InteractPlayer = InteractPlayers[i];
            float dis = Vector3.Distance(InteractPlayer.transform.position, transform.position);
            Moves.ForEach((MovementAbility ability) =>
            {
                if(ability.NeedObject)
                 ability.Active(dis, InteractPlayer, ability.Allowed(AllowedMoves));
            });
        }

        Moves.ForEach((MovementAbility ability)=>
        {
            if (!ability.NeedObject)
            {
                ability.Active(-1, null, ability.Allowed(AllowedMoves));
            }
        });

        if (Jump && AllowedMoves == 0 && OnGround) SPMovement.InitJump();
        if(AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, Run,1);
        if(AllowedMoves == 1) SPMovement.SimpleMovement(XAxis, YAxis, Run,PInfo.SpeedMultInAir);
       // MaxSpeed();
    }

    private void FrameAnim()
    {
        Animator Anim = PInfo.Anim;

        Anim.SetBool("onGround", checkGround(MaxGroundDis*4));

        Anim.SetBool("isWalking", false);
        Anim.SetInteger("stateRunning", 0);
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

    private bool checkGround(float length)
    {
        RaycastHit hit;
        bool hit1 = Physics.Raycast(groundTrans.position, -transform.up, out hit, length, groundMask);
        if (hit1)
        {
            Debug.Log(hit.normal);
        }

        return hit1;
    }
}

public class comp : IComparer<InteractPlayer>
{
    public int Compare(InteractPlayer x, InteractPlayer y)
    {
        return x.transform.position.magnitude.CompareTo(y.transform.position.magnitude);
    }
}