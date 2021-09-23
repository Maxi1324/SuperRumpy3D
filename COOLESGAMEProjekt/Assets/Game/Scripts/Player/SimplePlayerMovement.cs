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

    public void SimpleMovement(float xAxis,float yAxis, bool run)
    {
        PlayerInfo PInfo = PManager.PInfo;
        Rigidbody Rb = PManager.PInfo.rb;

        Rb.AddForce(transform.forward * Time.deltaTime * PInfo.WalkSpeed*yAxis);
        Rb.AddForce(transform.right * Time.deltaTime * PInfo.WalkSpeed*xAxis);
        Debug.Log("forceadded");

    }
}
