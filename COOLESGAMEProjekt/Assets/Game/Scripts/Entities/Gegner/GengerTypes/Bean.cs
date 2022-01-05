using Enemy.MovementDefinitions;
using Entity;
using Entity.Player;
using Generell.SoundManagement;
using System;
using System.Collections;
using UnityEngine;
namespace Enemy.Types
{
    public class Bean : Gegner, IDamageHandler
    {
        public Transform Center;
        public float Speed;
        public float Radius;

        private MovementCircle MC;

        public AudioSource AS1;
        public AudioSource AS2;
        public AudioSource AS3;

        public override IDamageHandler DamageHandler => this;

        public int aktLeben => 1;

        

        public void Die()
        {
            Anim.Play("Die2", 0);
            StartCoroutine(die());
        }

        public void Heal()
        {
            throw new NotImplementedException();
        }

        public void Hit(Vector3 EintrittsDir, Vector3 Rueckstoss, bool showAnimation, int DamageMultiplikator)
        {
            Die();
        }

        public override void InitMovements()
        {
            MC = new MovementCircle();
            MC.Target = Center;
            AddMovement(MC);
            ChangeMovement(MC);
        }

        public void Spawn(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public override void Update2()
        {
            MC.Speed = Speed;
            MC.Radius = Radius;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Entity1 ent = collision.transform.GetComponent<Entity1>();
            if (ent != null)
            {
                Vector3 dir = (collision.transform.position - transform.position).normalized;
                if (dir.y > 0.6f)
                {
                    collision.gameObject.GetComponent<PlayerManager>()?.PInfo.Rb.AddForce(new Vector3(dir.x, 0.6f, dir.z) * 3000);
                    Die();
                }
                else
                {
                    Vector3 rueckstoss = new Vector3(dir.x, 0.05f, dir.z) * 3000;
                    Debug.DrawLine(transform.position, dir * 100000, Color.red);
                    ent.DamageHandler.Hit(dir, rueckstoss, true, 1);
                }
            }
        }

        IEnumerator Steps()
        {
            bool Toggle = false;
            while (gameObject.activeSelf)
            {
                if (Toggle)
                {
                    AS1.Play();
                }
                else
                {
                    AS2.Play();
                }
                Toggle = !Toggle;
                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator die()
        {
            AS3.Play();
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);

        }

        public override void Start2()
        {
            StartCoroutine(Steps());
        }
    }
}
