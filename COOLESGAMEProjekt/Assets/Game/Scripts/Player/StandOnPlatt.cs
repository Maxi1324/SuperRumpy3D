using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Transform parent = transform.parent;
        if (Vector3.Distance(lastCol, transform.position) > dis)
        {
            transform.parent = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (PManager.onGround)
        {
            transform.parent = collision.transform;
            lastCol = transform.position;
        }
    }
}
