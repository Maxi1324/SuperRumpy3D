using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UI;
using Entity.Player;
using System.Collections;
using UI.CharacterSelection;
using Camera;

namespace Generell.LevelManager1
{
    public abstract class LevelManager: MonoBehaviour
    {
        public abstract void ResetLevel();
        public List<Transform> SpawnPoints = new List<Transform>();
        public GameObject PlayerPrefab;

        public Transform cam;

        public List<UIPlayerInfo> UIPlayerInfo { get; set; } = new List<UIPlayerInfo>();

        public int Coins;
        public static int Timer;

        private bool inited = false;

        GameObject StartC;
        GameObject EndC;

        private Vector3 startOff;

        public bool InCutScene { get; private set; }

        protected void Start()
        {
            startOff = CameraControllerGood.Instance.Offset;
            StartC = GameObject.Find("StartC");
            EndC = GameObject.Find("EndC");

            if (EndC != null)
            {
                EndC.SetActive(false);
            }
            if (StartC != null)
            {
                InCutScene = true;
            }
            else
            {
                StartEvent();
            }
        }

        public void TriggerEndCutScene()
        {
            InCutScene = true;
            EndC.SetActive(true);
            PlayerManager[] PM = FindObjectsOfType<PlayerManager>();
            for (int i = 0; i < PM.Length; i++)
            {
                Destroy(PM[i].gameObject);
            }
        }

        public void StartEvent()
        {
            if (StartC != null)
            {
                StartC.SetActive(false);
            }
            InCutScene = false;
            UIPlayerInfo = CharacterSelectionManager.Players;
            StartCoroutine(TimerSR());
            InitLevel();
        }

        public void InitLevel()
        {
            if (InCutScene) return;
            if (inited)
            {
                SceneLoader.Transition(false, () =>
                {
                    ResetLevel();
                    Spawn();
                    SceneLoader.Transition(true, () => { });
                });
            }
            else
            {
                Spawn();
                inited = true;
            }
            CameraControllerGood.Instance.Offset =startOff;
            CameraControllerGood.Instance.UpdatePlayers();
        }

        private void Spawn()
        {
            System.Random rnd = new System.Random();
            SpawnPoints.OrderBy((item) => rnd.Next());

            PlayerManager[] PM = FindObjectsOfType<PlayerManager>();
            for (int j = 0; j < PM.Length; j++)
            {
                Destroy(PM[j].gameObject);
            }

            DieManager Man = FindObjectOfType<DieManager>();
            Man.informed = false;

            int i = 0;
            UIPlayerInfo.ForEach((UIPlayerInfo PlayerInfo) =>
            {
                SpawnPlayer(PlayerInfo, SpawnPoints[i]);
                i++;
            });
            CameraControllerGood cont = FindObjectOfType<CameraControllerGood>();
            cont.FindPlayers();
        }

        private void SpawnPlayer(UIPlayerInfo PlayerInfo, Transform SpawnPoint)
        {
            GameObject ob = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation,null);
            PlayerManager PlayerManager = ob.GetComponent<PlayerManager>();
            PlayerInfo Info = PlayerManager.PInfo;
            Info.ControllerNum = PlayerInfo.ControllerNum;
            Info.PlayerNum = PlayerInfo.PlayerNum;
            Info.Skin = PlayerInfo.Skin;
            Info.Camera = cam;
        }

        IEnumerator TimerSR()
        {
            while (true)
            {
                Timer++;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
