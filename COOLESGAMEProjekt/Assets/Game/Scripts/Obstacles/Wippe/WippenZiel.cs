using System;
using System.Collections;
using UnityEngine;

namespace Obstacles.Wippe
{
    public class WippenZiel:MonoBehaviour
    {
        public Rigidbody rb;
        bool richtung;
        public float speed;
        bool dirChangeable = true;
        public float downSpeed = 2;

        private Vector3 dir;

        private void Start()
        {
            dir = transform.right;
            dir = new Vector3(dir.x,0, dir.z).normalized;
        }

        private void Update()
        {
            rb.AddForce(dir * speed*( richtung ? 1 : -1)* Time.deltaTime);
            rb.AddForce(Vector3.down*downSpeed*Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (dirChangeable && collision.transform.tag == "Stop")
            {
                StartCoroutine(r());
            }
        }

        IEnumerator r()
        {
            dirChangeable = false;
            rb.velocity = new Vector3();   
            richtung = !richtung;
            yield return new WaitForSeconds(2);
            dirChangeable = true;
        }
    }
}
