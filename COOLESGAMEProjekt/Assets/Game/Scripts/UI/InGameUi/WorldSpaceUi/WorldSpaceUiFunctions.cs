using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace UI.InGameUi
{
    public class WorldSpaceUiFunctions:MonoBehaviour
    {
        public Canvas canvas;
        public Canvas WSAlwaysOnTopCanvas;
        public GameObject TextPrefab;

        public static WorldSpaceUiFunctions Instance;

        public List<Tooltip> Tooltips = new List<Tooltip>();
        private Vector3 TooltipSize = new Vector3(.04f, .04f, .04f);

        [Serializable]
        public struct Tooltip
        {
            public GameObject ToolTipPrefab;
            public InGameUi.Tooltip tooltip;
        }

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
        }

        public TextMeshProUGUI ShowText(string str)
        {
            GameObject ob = Instantiate(TextPrefab);
            ob.transform.parent = canvas.transform;
            TextMeshProUGUI text = ob.GetComponent<TextMeshProUGUI>();
            text.text = str;
            return text;
        }

        public GameObject ShowTooltip(InGameUi.Tooltip TTType, Vector3 Position)
        {
            GameObject ob = Instantiate((Tooltips.First((Tooltip) => Tooltip.tooltip == TTType).ToolTipPrefab),Position,Quaternion.identity);
            ob.transform.localScale = TooltipSize;
            ob.transform.parent = WSAlwaysOnTopCanvas.transform;

            StartCoroutine(FaceCamera(ob.transform, WSAlwaysOnTopCanvas.worldCamera.transform));
            StartCoroutine(InGameUiFunktions.Instance.ScaleUD(true, ob.transform.localScale, () =>
            {
                Pulsieren(true, ob.transform.localScale, ob, 0.75f, 0.75f);
            }, ob,0,.8f));
            return ob;
        }

        public void DestroyTooltip(GameObject Tooltip, Action Fertig)
        {
            StartCoroutine(InGameUiFunktions.Instance.ScaleUD(false, Tooltip.transform.localScale, () =>
            {
                Destroy(Tooltip);
                Fertig();
            }, Tooltip, 0, .7f));
        }

        void Pulsieren(bool InOrOut, Vector3 LS,GameObject ob, float min, float t)
        {
            StartCoroutine(InGameUiFunktions.Instance.ScaleUD(InOrOut, LS, () =>
            {
                Pulsieren(!InOrOut, LS, ob, min,t);
            },ob,min,t));
        }

        private void Start()
        {
            Instance = this;
        }

        IEnumerator FaceCamera(Transform trans, Transform cam)
        {
            while(trans != null)
            {
                trans.LookAt(cam.transform.position);
                yield return null;
            }
        }

        public IEnumerator FollowTransform(Transform transgoal, Transform transfollower)
        {
            while (transgoal != null && transfollower != null)
            {
                transfollower.position = transgoal.position;
                yield return null;
            }
        }
    }

    [Serializable]
    public enum Tooltip
    {
        A
    }
}
