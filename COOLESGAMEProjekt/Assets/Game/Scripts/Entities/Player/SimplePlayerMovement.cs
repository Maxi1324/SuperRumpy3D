using Generell.SoundManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Player
{
    public class SimplePlayerMovement : MonoBehaviour
    {
        public PlayerManager PManager;
        public Quaternion? LookDir { get; set; }
        public float t = 0.1f;

        private bool JumpNotPressedUp = false;
        private bool jumpStarted = false;
        private bool wasNotOnGround = false;

        private float timer = 0;

        private float runningTimer = 0;

        private float TimeWhenStarted;

        private bool isWaliking = false;
        private float stepSpeed = .1f;

        private void Reset()
        {
            PManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            StartCoroutine(Steps());
        }

        private void FixedUpdate()
        {
            if (!(PManager.AllowedMoves == 1 || PManager.AllowedMoves == 0)) return;
            if (LookDir != null) smothRotation();

            if (!PManager.OnGround) wasNotOnGround = true;
            if (jumpStarted)
            {
                PlayerInfo PInfo = PManager.PInfo;
                Rigidbody rb = PInfo.Rb;

                float vy = rb.velocity.y;
                if (!PManager.Jump) JumpNotPressedUp = false;
                if (JumpNotPressedUp && vy > 0)
                {
                    Ray ray = new Ray(transform.position, Vector3.down);
                    //RaycastHit rayHit;


                    //float dis = Physics.Raycast(ray, out rayHit, Mathf.Infinity, PManager.groundMask) ? rayHit.distance : 1000;
                    // Debug.DrawRay(ray.origin, ray.direction * rayHit.distance, Color.green);

                    float timeInAir = Time.time - TimeWhenStarted;

                    float mult = (-Mathf.Log10(timeInAir * .5f) + .9f) * 3;

                    PInfo.Rb.AddForce(Vector3.up * 100 * PInfo.Rb.mass * Time.deltaTime * (mult > 0 ? mult : 1));
                    //PInfo.Rb.AddForce(Vector3.up * 1000 * PInfo.Rb.mass * Time.deltaTime);
                }

                if (PManager.OnGround && (timer > 5 || wasNotOnGround))
                {
                    PManager.AllowedMoves = 0;
                    JumpNotPressedUp = false;
                    jumpStarted = false;
                }
                timer = timer + Time.deltaTime;
            }
            //if (PManager.AllowedMoves == 0 && !PManager.OnGround) PManager.AllowedMoves = 1;

        }

        public void smothRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookDir ?? Quaternion.identity, t);
            if (LookDir == transform.rotation)
            {
                LookDir = null;
            }
        }

        public void SimpleMovement(float xAxis, float yAxis, bool run, float speedMult)
        {
            PlayerInfo PInfo = PManager.PInfo;
            Rigidbody Rb = PManager.PInfo.Rb;

            float speed = (run) ? (((runningTimer > 3)) ? PInfo.RunFastSpeed : PInfo.RunSpeed) : PInfo.WalkSpeed;
            speed *= speedMult;

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
            if (L < 1 && L > -0.4f)
            {
                Vector3 mVecNorm = moveVec.normalized;
                float multV = 1 - (L);
                if (multV < 0) multV = 0;
                if (multV > 1) multV = 1;
                Rb.AddForce(mVecNorm * Time.deltaTime * multV * PInfo.Beschleunigung * mult * Rb.mass);
            }

            if (xAxis != 0 || yAxis != 0)
            {
                PInfo.Anim.SetBool("isWalking", true);
                if (run) PInfo.Anim.SetInteger("stateRunning", (runningTimer > 3) ? 2 : 1);
                runningTimer += Time.deltaTime;
                isWaliking = true;
                stepSpeed = run ? .3f : .5f;
            }
            if(xAxis == 0 && yAxis == 0)
            {
                isWaliking = false;
            }
            if (!run)
            {
                runningTimer = 0;
            }
        }

        IEnumerator Steps()
        {
            bool Toggle = false;
            while (true)
            {
                if (isWaliking && PManager.OnGround == true)
                {
                    SoundManager.Play("Step" + (Toggle ? "1" : "2"));
                    Toggle = !Toggle;
                    yield return new WaitForSeconds(stepSpeed);
                }
                else
                {
                    yield return null;
                }
            } 
        }

        public void StartJump()
        {
            PlayerInfo PInfo = PManager.PInfo;
            Rigidbody RB = PInfo.Rb;
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            RB.AddForce(transform.rotation * PInfo.JumpForce * RB.mass);
            TimeWhenStarted = Time.time;
        }

        public void InitJump()
        {
            if (jumpStarted == false)
            {
                PlayerInfo PInfo = PManager.PInfo;
                float v = PInfo.Rb.velocity.magnitude;
                PManager.AllowedMoves = 1;
                JumpNotPressedUp = true;
                jumpStarted = true;
                timer = 0;
                wasNotOnGround = false;
                PManager.PInfo.Anim.SetBool("doNotEnterAirIdle", true);
                PManager.PInfo.Anim.SetBool("isJumping", true);
            }
        }
    }
}