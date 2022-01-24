using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

namespace Entity.Player.Abilities
{
    public class LongJump : MovementAbility
    {
        public PlayerManager PM;
        public Vector3 Dir;
        public bool isLongJumping;
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
            if(isLongJumping)
            {
                PM.AllowedMoves = 5;
                PM.SPMovement.SimpleMovement(PM.XAxis, PM.YAxis, false, .8f);
            }  
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed && PM.Bumper && PM.JumpPressed)
            {

                PM.AllowedMoves = -1;
                PM.PInfo.Anim.SetBool("StartLongJump", true);
                PM.PInfo.Anim.Play("StartLongJump");
                isLongJumping = true;
                return true;
            }
            return false;
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0 && PM.Run && PM.OnGround;
        }

        public override void HelperFunction()
        {
            Debug.Log("ahal");
            PM.PInfo.Rb.AddForce(transform.rotation* Dir);
            PM.AllowedMoves = 5;
           // PM.PInfo.Anim.SetBool("StartLongJump", false);
            PM.PInfo.Anim.SetBool("IsJumping", false);
            StartCoroutine(MaxTime());
        }

        public override void HelperFunction2()
        {
            PM.AllowedMoves = 0;
            PM.PInfo.Anim.SetBool("StartLongJump", false);
            isLongJumping = false;
        }

        IEnumerator MaxTime()
        {
            yield return new WaitForSeconds(4);
            if (PM.PInfo.Anim.GetBool("StartLongJump"))
            {
                HelperFunction2();
                PM.AllowedMoves = 1;
            }
        }
    }
}