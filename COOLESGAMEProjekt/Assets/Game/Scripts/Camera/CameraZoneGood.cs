using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class CameraZoneGood : MonoBehaviour
    {
        public Vector3 Offset;
        public bool relativeOffset = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                CameraControllerGood.Instance.Offset = (relativeOffset) ? transform.rotation * Offset : Offset;
            }
        }
    }
}
/*
 * PlayerEntdecken also Collision
 * Offset des CameraControllers ändern
 */