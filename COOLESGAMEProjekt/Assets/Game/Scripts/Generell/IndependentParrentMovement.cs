using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generell
{
    public class IndependentParrentMovement : MonoBehaviour
    {
        private Dictionary<Transform, Vector3> lastPoses = new Dictionary<Transform, Vector3>();

        private void Start()
        {

        }

        private void FixedUpdate()
        {
            Transform[] obs = gameObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform trans in obs)
            {
                if (lastPoses.ContainsKey(trans))
                {

                    if (trans != transform)
                    {
                        Vector3 lastPos = lastPoses[trans];
                        Vector3 diff = transform.position - lastPos;
                        trans.transform.position = diff;
                        lastPoses[trans] = transform.position;
                    }
                }
                else
                {
                    lastPoses.Add(trans, transform.position);
                }
            }
        }
    }
}