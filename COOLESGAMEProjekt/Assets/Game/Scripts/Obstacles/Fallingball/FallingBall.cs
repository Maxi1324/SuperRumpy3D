using Entity.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles.FallingBall
{
    public class FallingBall : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine("End");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == "Player")
            {
                Debug.Log("cooll");
                PlayerManager PM = collision.transform.GetComponent<PlayerManager>();
                PM.Hit(Vector3.zero, Vector3.zero, true,1);
            }
        }

        IEnumerator End()
        {
            yield return new WaitForSeconds(60);
            Destroy(gameObject);
        }
    }
}