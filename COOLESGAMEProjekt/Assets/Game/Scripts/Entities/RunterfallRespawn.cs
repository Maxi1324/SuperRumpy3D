using Entity.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Entity
{
    class RunterfallRespawn : MonoBehaviour
    {
        public Vector3 lastSafePosition { get; set; }
        public Entity1 Ent;
        public Rigidbody Rb;
        public GameObject Partikel;

        public int mult = 1;

        public LayerMask Mask;

        private void Reset()
        {
            Ent = GetComponent<Entity1>();
            Rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            lastSafePosition = transform.position;
            StartCoroutine(FindPos());
        }

        public void BacktoSafePosition()
        {
            bool s = gameObject.activeSelf;
            gameObject.SetActive(true);
            StartCoroutine(Teleport());
            gameObject.SetActive(s);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Kill")
            {
                Ent.DamageHandler.Hit(Vector3.zero, Vector3.zero, false, mult);
                PlayerManager PM = (Ent as PlayerManager);
                if (PM != null)
                {
                    PM.PInfo.Renderer.enabled = false;
                    PM.Invincible = true;
                    PM.AllowedMoves = -1;
                }
                BacktoSafePosition();
            }
            if(collision.transform.tag == "lava")
            {
                //Ent.DamageHandler.Hit(Vector3.zero, Vector3.zero, false, mult);
                //Ent.DamageHandler.Hit(Vector3.zero, Vector3.zero, false, mult);
                Ent.DamageHandler.Die();
            }
        }

        IEnumerator FindPos()
        {
            while (true)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask) )
                {
                    if (hit.normal.y > 0.9 &&  hit.transform.tag == "Map")
                    {
                        //lastSafePosition = transform.position;
                        yield return null;
                    }
                }
                yield return null;
            }
        }

        IEnumerator Teleport()
        {
            PlayerManager PM = (Ent as PlayerManager);
            bool NowDead = Ent.DamageHandler.aktLeben <= 0;
            if (PM != null)
            {
                PM.PInfo.Renderer.enabled = false;
                PM.Invincible = true;
                PM.AllowedMoves = -1;
            }
            VisualEffect sys = null;
            if (!NowDead)
            {
                GameObject ob = Instantiate(Partikel, transform.position, Quaternion.identity);
                sys = ob.GetComponent<VisualEffect>();
            }
            yield return new WaitForSeconds(2);
            if (!NowDead)
            {
                StartCoroutine("DestroyParticle", sys);
            }
            transform.position = lastSafePosition;
            if (PM != null)
            {
                PM.PInfo.Renderer.enabled = true;
                PM.Invincible = false;
                PM.AllowedMoves = 0;
            }
        }

        IEnumerator DestroyParticle(VisualEffect Sys)
        {
            while (Sys.aliveParticleCount > 10)
            {
                yield return null;
            }
            Destroy(Sys.gameObject);
        }
    }
}