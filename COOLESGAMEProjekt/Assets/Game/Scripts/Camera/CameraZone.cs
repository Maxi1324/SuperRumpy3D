using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CameraZone : MonoBehaviour
{
    public Vector3 BounceSize;
    public List<Vector3> offets = new List<Vector3>(1);
    public CameraController CController;

    private bool collided = false;

    private void Reset()
    {
        CController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        if (CController == null) Reset();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            CController.toTrack(offets);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CController.clear();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        //Gizmos.DrawCube(transform.position+GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size*0.575f);
    }
#endif

}