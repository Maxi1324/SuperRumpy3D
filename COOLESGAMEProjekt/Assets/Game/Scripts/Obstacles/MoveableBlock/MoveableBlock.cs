using System;
using UnityEngine;

namespace Obstacles.MoveableBlocks
{
    public class MoveableBlock : MonoBehaviour
    {
        public Rigidbody rb;
        public float abstandBoden = 2;

        private Vector3 dir;
        private bool active = true;
        public int typ = 0;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            RaycastHit hit;
            if(Physics.Raycast(transform.position,Vector3.down,out hit, Mathf.Infinity))
            {
                transform.position = hit.point + Vector3.up* abstandBoden;
            }
            coll.enabled = true;
        }


        public void Stop()
        {
            active = false;
            rb.velocity = Vector3.down*8;
        }

        private void Update()
        {
            if (active)
            {
                rb.velocity = dir * 100;
            }
        }

        public void Move(Transform Mover)
        {
            if (rb.velocity.magnitude < 1)
            {
                dir = -(Mover.position - transform.position).normalized;
                if (Mathf.Abs(dir.x) > Math.Abs(dir.z))
                {
                    dir = new Vector3(dir.x, -.3f, 0).normalized;
                }
                else
                {
                    dir = new Vector3(0, -.1f, dir.z).normalized;
                }
            }
        }
    }
}
