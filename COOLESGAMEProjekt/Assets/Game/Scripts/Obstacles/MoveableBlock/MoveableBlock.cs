using Entity.Player;
using System;
using System.Collections;
using UI.InGameUi;
using UnityEngine;
using UnityEngine.VFX;

namespace Obstacles.MoveableBlocks
{
    public class MoveableBlock : MonoBehaviour
    {
        public Rigidbody rb;
        public float abstandBoden = 2;

        private Vector3 dir;
        private bool active = true;
        public int typ = 0;

        public GameObject Partikel;

        private Action OnFertig;

        private Vector3 StartPos;

        public bool ShouldRespawn;
        public int RespawnTime;

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

            StartPos = transform.position;
        }


        public void Stop()
        {
            active = false;
            rb.velocity = Vector3.down*8;
            OnFertig();
        }

        private void Update()
        {
            if (active)
            {
                rb.velocity = dir * 60;
            }
        }

        public void ResetBlock()
        {
            Stop();
            Vector3 sScale = transform.localScale;
            GameObject hallo = Instantiate(Partikel, transform.position, Quaternion.identity);
            StartCoroutine(DestroyParticle(hallo.GetComponent<VisualEffect>()));
            StartCoroutine(InGameUiFunktions.Instance.ScaleUD(false, sScale, ()=> {
               
                transform.position = StartPos;
                StartCoroutine(InGameUiFunktions.Instance.ScaleUD(true, sScale, () => {

                }, gameObject, 0, 0.9f));
            },gameObject,0,0.9f));
        }

        public Vector3 Move(Transform Mover,Action OnFertig)
        {
            if (rb.velocity.magnitude < 1)
            {
                active = true;
                this.OnFertig = OnFertig;
                dir = -(Mover.position - transform.position);
                Vector3 dirN = dir.normalized;
                if (Mathf.Abs(dirN.x) > Math.Abs(dirN.z))
                {
                    dir = new Vector3(dir.x, -.3f, 0).normalized;
                }
                else
                {
                    dir = new Vector3(0, -.1f, dirN.z).normalized;
                }
                if ((Math.Sign(dir.x) < 0.6 && Math.Sign(dir.y) < 0.6))
                {
                   // dir = Vector3.zero;
                }
                if (ShouldRespawn)
                {
                    StartCoroutine(DoInNSec(() =>
                    {
                        ResetBlock();
                    }, RespawnTime));
                }
                return dir;
            }
            return Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            MoveableBlock MB = collision.transform.GetComponent<MoveableBlock>();
            if(MB != null)
            {
                ResetBlock();
            }
        }

        IEnumerator DestroyParticle(VisualEffect Sys)
        {
            while (Sys != null && Sys.aliveParticleCount > 10)
            {
                yield return null;
            }
            if (Sys != null)
            {
                Destroy(Sys.gameObject);
            }
        }

        public static IEnumerator DoInNSec(Action ac, int n)
        {
            yield return new WaitForSeconds(n);
            ac();
        }
    }
}
