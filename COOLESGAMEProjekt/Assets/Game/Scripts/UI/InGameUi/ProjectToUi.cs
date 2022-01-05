using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGameUi {

    public class ProjectToUi : MonoBehaviour
    {
        public Transform InGamePosition;
        public Transform UiPosition;

        
        private void Update()
        {
            UiPosition.position = InGamePosition.position;
        }
    }
}