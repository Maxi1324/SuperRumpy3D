using Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Obstacles.Events
{
    public class EventTrigger : MonoBehaviour
    {
        public UnityEvent Event;
        public bool OneTimer = false;
        public bool TriggerByColl = true;

        public bool started { get; private set; }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && TriggerByColl)
            {
                TriggerEvent();
            }
        }

        public void TriggerEvent()
        {
            if (OneTimer && !started)
            {
                started = true;
                Event.Invoke();
                CameraControllerGood CaCo = FindObjectOfType<CameraControllerGood>();
                CaCo.UpdatePlayers();
            }
        }
    }
}