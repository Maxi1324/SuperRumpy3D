using System;
using UnityEngine;

namespace Entity.Player
{
    public class SchadenByTag : MonoBehaviour
    {
        public Entity1 ent;
        public string tag1 = "Hit";
        public int mult = 1;

        private void Reset()
        {
            ent = GetComponent<Entity1>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (tag1 == collision.transform.tag)
            {
                Vector3 dir = (collision.transform.position - transform.position).normalized;
                Vector3 rueckstoss = new Vector3(dir.x, 0.1f, dir.z) * 3000;
                ent.DamageHandler.Hit(dir, rueckstoss, true, mult);
            }
        }
    }
}