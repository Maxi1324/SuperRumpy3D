using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Entity.Player.Extras
{
    public class StandOnPlatt : MonoBehaviour
    {
        public PlayerManager PManager;
        public float dis = 10;

        private Vector3 lastCol;

        private void Reset()
        {
            PManager = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            
        }
        // neu Implementieren
        private void OnCollisionStay(Collision collision)
        {
           
        }
    }
}
