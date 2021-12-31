using Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entity.Player.Abilities
{
    public class BetterJump : MovementAbility
    {
        public Vector3 JumpForce;
        public PlayerManager PManager;

        public float TimeToWait = 1;
        public float LoadingTime = 4;
        private float timer;
        private float timer2;


        private void Reset()
        {
            PManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            NeedObject = true;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed)
            {
                if (PManager.Fire3)
                {
                    timer += Time.deltaTime;
                    if (timer > TimeToWait)
                    {
                        PManager.AllowedMoves = 3;
                        timer2 += Time.deltaTime;
                        if (timer2 > LoadingTime)
                        {
                            doJump();
                        }
                    }
                }
                else
                {
                    if (timer > TimeToWait)
                    {
                        doJump();
                    }
                    timer = 0;
                    timer2 = 0;
                }
            }
            else
            {
                timer = 0;
                timer2 = 0;
            }
            return false;
        }

        private void doJump()
        {
            if (!PManager.OnGround) return;
            PManager.PInfo.Rb.AddForce(JumpForce);
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0 || allowedMoves == 3;
        }

        void Update()
        {

        }
    }
}
