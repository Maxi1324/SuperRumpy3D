using System.Collections;
using System.Collections.Generic;
using UI.InGameUi;
using UnityEngine;
using TMPro;
using System;

namespace Obstacles.Schild
{
    public class Schild : MonoBehaviour
    {
        [Multiline]
        public string text;
        public Transform trans;

        private bool eins;

        private TextMeshProUGUI Text;

        private void Start()
        {
            Text = WorldSpaceUiFunctions.Instance.ShowText(text);
            Text.transform.position = trans.position;
            Text.transform.rotation = trans.rotation;
            Text.transform.localScale = Text.transform.localScale / 45.82634f;
            Text.transform.localScale *= trans.localScale.magnitude;
        }
        //15
        public void show(Action Fertig )
        {
            if (eins) return;
            eins = true;
            //InGameUiFunktions.Instance.ShowText(text,Fertig);
        }
    }
}