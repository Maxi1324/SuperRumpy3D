using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Env
{
    public class Kette : MonoBehaviour
    {
        public GameObject KettenStueck;
        public Vector3 dis;
        public Transform start;
        public bool useRay;
        public LayerMask mask;
        public int num = 10;

        void Start()
        {
            if (useRay)
            {
                RaycastHit info;
                if (Physics.Raycast(start.position, Vector3.up, out info, Mathf.Infinity, mask))
                {
                    num = (int)(info.distance / 2) + 1;
                }
                else
                {
                    Debug.LogError("Diggi, das ist gar keine Decke mach das mal(" + transform.name + ")");
                }
            }
            Rigidbody lastRig = null;
            for (int i = num - 1; i >= 0; i--)
            {
                Vector3 off = dis * i;
                GameObject LK = Instantiate(KettenStueck, start.position + off, Quaternion.Euler(0, 90 * i, 0), transform.parent);
                LK.transform.localScale = new Vector3(20.15453f, 20.15453f, 20.15453f) * 3.7832f;

                Joint joint = LK.GetComponent<Joint>();
                if (lastRig != null) joint.connectedBody = lastRig;

                if (i == num - 1)
                {
                    Rigidbody rb = LK.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                }
                if (i == 0)
                {
                    Rigidbody rb = LK.GetComponent<Rigidbody>();
                    rb.mass = 100;
                    rb.constraints = RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ;

                    transform.parent = LK.transform;
                }
                lastRig = LK.GetComponent<Rigidbody>();
            }
        }

        void Update()
        {

        }
    }
}