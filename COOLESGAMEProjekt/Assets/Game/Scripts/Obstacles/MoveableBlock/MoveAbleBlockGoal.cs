using Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Obstacles.MoveableBlocks;
using UnityEngine.VFX;

public class MoveAbleBlockGoal : MonoBehaviour
{
    public UnityEvent event1;
    public int typ;


    private void OnCollisionEnter(Collision collision)
    {
        MoveableBlock interact = collision.gameObject.GetComponent<MoveableBlock>();
        if (interact != null)
        {
            if (typ == interact.typ)
            {
                event1.Invoke();
                interact.Stop();
                BoxCollider coll = collision.collider as BoxCollider;
                if (coll != null)
                {
                    coll.size = coll.size * 0.6f;
                }
                interact.GetComponent<Rigidbody>().velocity = new Vector3();
                interact.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX
                                                                    | RigidbodyConstraints.FreezeRotationY
                                                                    | RigidbodyConstraints.FreezeRotationZ
                                                                    | RigidbodyConstraints.FreezePositionX
                                                                    | RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                interact.ResetBlock();
            }
        }

       
    }
}
