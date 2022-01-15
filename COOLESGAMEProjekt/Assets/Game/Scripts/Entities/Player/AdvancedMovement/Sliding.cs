using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

/// <summary>
/// Prüfen, ob soll Sliden
/// Siliden Bewegen
/// </summary>

namespace Entity.Player.Abilities
{
    public class Sliding : MovementAbility
    {
        public PlayerManager PM;
        [Range(0,1)]
        public float SollRutschen;
        public float t;
        public float mult;

        private bool IsSliding;

        private float size;

        private void Start()
        {
            NeedObject = false;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed && !IsSliding)
            {
                StartRutschen();
            }
            return true;
        }

        public override bool Allowed(int allowedMoves)
        {
            return PM.OnGroundNormal.y < SollRutschen;
        }

        public void StartRutschen()
        {
            PM.AllowedMoves = 4;
            IsSliding = true;
            PM.PInfo.Anim.SetBool("IsSliding", true);
            PM.PInfo.Collider.material = PM.PInfo.PMaterialSliding;
            StartCoroutine(Rutschen());
            CapsuleCollider col = PM.PInfo.Collider as CapsuleCollider;
            size = col.height;
            col.height = 0;
            col.center = new Vector3(0,.5f,0);
        }

        public void StopRutschen()
        {
            PM.PInfo.Collider.material = PM.PInfo.PMaterialNormal;
            CapsuleCollider col = PM.PInfo.Collider as CapsuleCollider;
            col.height = 0;
            col.center = new Vector3();
        }

        IEnumerator Rutschen()
        {
            while (IsSliding) {
                Quaternion goalQuat = Quaternion.FromToRotation(Vector3.up, PM.OnGroundNormal);
                goalQuat = Quaternion.Lerp(transform.rotation, goalQuat, t);
                Vector3 rot1 = goalQuat.eulerAngles;
                transform.rotation = Quaternion.Euler(rot1.x, transform.rotation.eulerAngles.y, rot1.z);

                rot(PM.PInfo.Rb.velocity.normalized,t, false, true, false);
                Move();
                yield return null;
            }
        }

        private void Move()
        {
            Rigidbody rb = PM.PInfo.Rb;
            rb.AddForce(transform.right * Time.deltaTime * PM.XAxis* mult);
            Debug.DrawLine(transform.position, transform.position+ PM.OnGroundNormal*10000, Color.red);
            rb.AddForce(PM.OnGroundNormal * Time.deltaTime * mult);
        }

        private void rot(Vector3 dir, float t, bool x, bool y, bool z)
        {
            Quaternion goalQuat = Quaternion.FromToRotation(Vector3.forward, dir);
            goalQuat = Quaternion.Lerp(transform.rotation, goalQuat, t);
            Vector3 rot = goalQuat.eulerAngles;
            Vector3 old = transform.root.eulerAngles;
            transform.rotation = Quaternion.Euler(x ? rot.x : old.x, y ? rot.y : old.y, z ? rot.z : old.z);
        }

        void Update()
        {

        }
    }
}