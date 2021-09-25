using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public PlayerManager PManager;
    public Quaternion? LookDir { get; set; }
    public float t = 0.1f;

    private bool JumpNotPressedUp = false;
    private bool jumpStarted = false;

    private float timer = 0;

    private void Reset()
    {
        PManager = GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        if (LookDir != null) smothRotation();
       
        if (jumpStarted)
        {
            PlayerInfo PInfo = PManager.PInfo;
            Rigidbody rb = PInfo.Rb;

            Debug.Log(JumpNotPressedUp);

            float vy = rb.velocity.y;
            if (!PManager.jump) JumpNotPressedUp = false;
            if (JumpNotPressedUp && vy > 0)
            {
                PInfo.Rb.AddForce(Vector3.up * 250 * PInfo.Rb.mass * Time.deltaTime);
            }

            if (PManager.onGround && timer > .2f)
            {
                PManager.AllowedMoves = 0;
                JumpNotPressedUp = false;
                jumpStarted = false;
            }
            timer = timer + Time.deltaTime;
        }
    }

    public void smothRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, LookDir??Quaternion.identity, t);
        if(LookDir == transform.rotation)
        {
            LookDir = null;
        }
    }

    public void SimpleMovement(float xAxis, float yAxis, bool run, float speedMult)
    {
        PlayerInfo PInfo = PManager.PInfo;
        Rigidbody Rb = PManager.PInfo.Rb;

        float speed = (run) ? PInfo.RunSpeed : PInfo.WalkSpeed;
        speed *= speedMult;
        Quaternion quaternion = Quaternion.Euler(0, PInfo.Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 turnDir = (quaternion * Vector3.forward * yAxis + quaternion * Vector3.right * xAxis).normalized;
        float mult = 1f;

        if ((turnDir - transform.forward).magnitude > .1f && (turnDir.magnitude > 0.05f))
        {
            Quaternion rotation = Quaternion.LookRotation(turnDir, Vector3.up);
            LookDir = rotation;
            mult = 0.4f;
        }

        Vector3 moveVec = (quaternion * Vector3.forward * speed * yAxis) +
        (quaternion * Vector3.right * speed * xAxis);
        if (moveVec.magnitude > speed) moveVec = moveVec.normalized * speed;
        Vector3 v = Rb.velocity;
        //Projektion v auf moveVec
        float L = Vector3.Dot(v, moveVec) / Vector3.Dot(moveVec, moveVec);
        if(L < 1 && L > -0.4f)
        {
            Vector3 mVecNorm = moveVec.normalized;
            float multV = 1-(L);
            Rb.AddForce(mVecNorm*Time.deltaTime*multV*PInfo.Beschleunigung* mult*Rb.mass);
        }
    }

    public void StartJump()
    {
        if (jumpStarted == false)
        {
            PlayerInfo PInfo = PManager.PInfo;
            float v = PInfo.Rb.velocity.magnitude;
            PInfo.Rb.AddForce(transform.rotation * PInfo.JumpForce * PInfo.Rb.mass);
            PManager.AllowedMoves = 1;
            JumpNotPressedUp = true;
            jumpStarted = true;
            timer = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "MovePlattform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "MovePlattform")
        {
            transform.parent = null;
        }
    }
}