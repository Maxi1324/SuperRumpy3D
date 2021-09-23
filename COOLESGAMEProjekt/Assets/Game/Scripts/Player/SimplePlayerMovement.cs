using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public PlayerManager PManager;

    private void Reset()
    {
        PManager = GetComponent<PlayerManager>();
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

        Rb.AddForce(quaternion * Vector3.forward * Time.deltaTime * PInfo.Beschleunigung * yAxis);
        Rb.AddForce(quaternion * Vector3.right * Time.deltaTime * PInfo.Beschleunigung * xAxis);
    }

    public void StartJump()
    {
        PlayerInfo PInfo = PManager.PInfo;

        PManager.maxSpeed = -1;
        Quaternion quaternion = Quaternion.Euler(0, PInfo.Camera.transform.rotation.eulerAngles.y, 0);

        PInfo.rb.AddForce(quaternion * PInfo.normalJumpForce);
        PManager.AllowedMoves = 1;
    }
}
