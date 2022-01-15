using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

namespace Entity.Player.Abilities {
    public class HighJump : MovementAbility
    {
        public PlayerManager PM;
        public Vector3 Dir;
        private void Reset()
        {
            PM = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            NeedObject = false;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed && PM.Bumper && PM.JumpPressed)
            {
                Debug.Log(allowed);

                PM.AllowedMoves = -1;
                PM.PInfo.Anim.SetBool("StartHighJump",true);
                return true;
            }
            return false;
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0 && !PM.Run;
        }

        public override void HelperFunction()
        {
            PM.PInfo.Rb.AddForce(Dir);
            PM.AllowedMoves = 0;
            PM.PInfo.Anim.SetBool("StartHighJump", false);
            PM.PInfo.Anim.SetBool("IsJumping", false);
            Debug.Log("lol");
        }
    }
}