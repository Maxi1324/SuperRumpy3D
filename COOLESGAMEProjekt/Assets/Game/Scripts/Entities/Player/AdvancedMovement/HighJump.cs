using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

namespace Entity.Player.Abilities {
    public class HighJump : MovementAbility
    {
        public PlayerManager PM;
        public Vector3 Dir;

        private bool isHighJumping;

        private void Reset()
        {
            PM = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            NeedObject = false;
        }

        private void Update()
        {
            if (isHighJumping)
            {
                PM.AllowedMoves = -1;
            }
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed && PM.Bumper && PM.JumpPressed)
            {
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
            PM.PInfo.Anim.SetBool("isJumping", false);
            isHighJumping = true;
        }

        public override void HelperFunction2()
        {
            PM.AllowedMoves = 0;
            PM.PInfo.Anim.SetBool("StartHighJump", false);
            isHighJumping = false;
        }
    }
}