using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity.Player.Abilities;

namespace Entity.Player.Extras
{
    public class PlayerAnimEvents : MonoBehaviour
    {
        public PlayerManager PM;

        public void doJump()
        {
            PM.SPMovement.StartJump();
            PM.PInfo.Anim.SetBool("isJumping", false);
            PM.PInfo.Anim.SetBool("doNotEnterAirIdle", false);
        }

        public void doLittleJump()
        {
            PM.PInfo.Rb.AddForce(new Vector3(0,100,0));
        }

        public void printText()
        {
            Debug.Log("halll0o");
            Time.timeScale = 0;
        }

        public void doHighJump()
        {
            MovementAbility MA = PM.Moves.Find(h => h is HighJump);
            MA.HelperFunction();
            StartCoroutine(disJump());
        }

        public void doLongJump()
        {
            MovementAbility MA = PM.Moves.Find(h => h is LongJump);
            MA.HelperFunction();
            StartCoroutine(disJump());
        }

        public void EndLongJump()
        {
            MovementAbility MA = PM.Moves.Find(h => h is LongJump);
            MA.HelperFunction2();
        }

        IEnumerator disJump()
        {
            yield return new WaitForSeconds(1);
            PM.PInfo.Anim.SetBool("isJumping", false);
        }
    }
}