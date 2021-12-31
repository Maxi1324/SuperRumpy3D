using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CharacterSelection
{
    public class UICreatePlayerStatisten : MonoBehaviour
    {
        public List<GameObject> StatistenModels = new List<GameObject>();
        private List<GameObject> Statisten = new List<GameObject>();
        public List<RenderTexture> RenderTextures = new List<RenderTexture>();

        public Vector3 CamOffset = new Vector3(0, 5, -10);

        private int offset = 100;

        public static UICreatePlayerStatisten Instance;

        public UICreatePlayerStatisten()
        {
            Instance = this;
        }

        public void Start()
        {
            int i = 0;
            StatistenModels.ForEach((GameObject Model) =>
            {
                GameObject Ob = null;
                Ob = Instantiate(Model, new Vector3(offset * (i), 0, 0), Quaternion.identity, transform);

                GameObject CamOb = new GameObject("CameraOB");
                CamOb.transform.parent = Ob.transform;
                CamOb.transform.position = CamOffset;
                CamOb.AddComponent(typeof(UnityEngine.Camera));
                CamOb.transform.rotation = Quaternion.Euler(0, 180, 0);
                var Cam = CamOb.GetComponent<UnityEngine.Camera>();

                RenderTexture RT = new RenderTexture(248*2, 175*2, 16, RenderTextureFormat.ARGB32);
                RT.Create();
                Cam.targetTexture = RT;
                Cam.clearFlags = CameraClearFlags.SolidColor;
                Cam.backgroundColor = new Color(0, 0, 0, 0);
                RenderTextures.Add(RT);
                Statisten.Add(Ob);
                i++;
            });
        }
    }
}