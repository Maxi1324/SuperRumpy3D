using Enemy.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Env
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;
        public float Intervall;
        public Transform Position;
        public int MaxObs;

        private float Timer;

        private List<GameObject> Instances = new List<GameObject>();

        private void Reset()
        {
            Position = gameObject.transform;
        }

        private void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > Intervall)
            {
                if (Instances.Count < MaxObs)
                {
                    GameObject ob = Instantiate(Prefab, Position.position, Position.rotation, transform);
                    initBean(ob);
                    Instances.Add(ob);
                    Timer = 0;
                }
            }
        }

        private void initBean(GameObject ob)
        {
            Bean bean = ob.GetComponent<Bean>();
            if (bean != null)
            {
                bean.Center = transform;
            }
        }
    }
}