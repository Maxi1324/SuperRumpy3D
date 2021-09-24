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
        Rigidbody Rb = PManager.PInfo.rb;

        float speed = (run) ? PInfo.RunSpeed : PInfo.WalkSpeed;
        if (PManager.maxSpeed != speed)
        {
            PManager.maxSpeed = speed;
        }

        Quaternion quaternion = Quaternion.Euler(0, PInfo.Camera.transform.rotation.eulerAngles.y, 0);

        // Rb.AddForce(quaternion * Vector3.forward * Time.deltaTime * PInfo.Beschleunigung * yAxis);
        //Rb.AddForce(quaternion * Vector3.right * Time.deltaTime * PInfo.Beschleunigung * xAxis);

        Vector3 turnDir = (quaternion * Vector3.forward * yAxis + quaternion * Vector3.right * xAxis).normalized;
        float mult = 1f;

        if ((turnDir - transform.forward).magnitude > .1f && (turnDir.magnitude > 0.05f))
        {
            //Debug.DrawRay(transform.position, turnDir, Color.green, 10);
            Quaternion rotation = Quaternion.LookRotation(turnDir, Vector3.up);
            // transform.rotation = rotation;
            LookDir = rotation;
            mult = 0.4f;
        }
        {
            Rb.AddForce(quaternion * Vector3.forward * Time.deltaTime * PInfo.Beschleunigung * yAxis * mult);
            Rb.AddForce(quaternion * Vector3.right * Time.deltaTime * PInfo.Beschleunigung * xAxis * mult);
        }
    }

    public void StartJump()
    {
        PlayerInfo PInfo = PManager.PInfo;
        float v = PInfo.rb.velocity.magnitude;

        PInfo.rb.velocity = PInfo.rb.velocity / 2;
        PInfo.rb.AddForce(transform.rotation * PInfo.normalJumpForce);
        PManager.maxSpeed = -1;
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