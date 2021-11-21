using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public PlayerManager PM;

    public void doJump()
    {
        PM.SPMovement.StartJump();
        PM.PInfo.Anim.SetBool("isJumping", false);
        PM.PInfo.Anim.SetBool("doNotEnterAirIdle", false);
    }

    public void printText()
    {
        Debug.Log("halll0o");
        Time.timeScale = 0;
    }
}
