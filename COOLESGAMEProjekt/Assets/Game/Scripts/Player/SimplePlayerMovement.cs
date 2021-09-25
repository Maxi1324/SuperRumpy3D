using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public PlayerManager PManager;
    public Quaternion? LookDir { get; set; }
    public float t = 0.1f;

    private void Reset()
    {
        PManager = GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        if (LookDir != null) smothRotation();

    }

    public void smothRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, LookDir??Quaternion.identity, t);
        if(LookDir == transform.rotation)
        {
            LookDir = null;
        }
    }

    public void SimpleMovement(float xAxis, float yAxis, bool run)
    {
        PlayerInfo PInfo = PManager.PInfo;
        Rigidbody Rb = PManager.PInfo.Rb;

        float speed = (run) ? PInfo.RunSpeed : PInfo.WalkSpeed;
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
        Debug.Log(L);
        if(L < 1)
        {
            Vector3 mVecNorm = moveVec.normalized;
            float multV = 1-L;
            Rb.AddForce(mVecNorm*Time.deltaTime*multV*PInfo.Beschleunigung* mult);
        }
    }

    public void StartJump()
    {
        PlayerInfo PInfo = PManager.PInfo;
        float v = PInfo.Rb.velocity.magnitude;

        PInfo.Rb.velocity = PInfo.Rb.velocity / 2;
        PInfo.Rb.AddForce(transform.rotation * PInfo.normalJumpForce);
        //PManager.maxSpeed = -1;
        PManager.AllowedMoves = 1;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == 7 || collision.transform.tag == "Map")
        {
            PManager.AllowedMoves = 0;
        }
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