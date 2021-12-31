using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UI;
using Entity.Player;

namespace Generell.LevelManager1
{
    public abstract class LevelManager: MonoBehaviour
    {
        public abstract void ResetLevel();
        public List<Transform> SpawnPoints = new List<Transform>();
        public GameObject PlayerPrefab;

        public Transform cam;

        public List<UIPlayerInfo> UIPlayerInfo { get; set; } = new List<UIPlayerInfo>();

        private bool inited = false;

        private void Start()
        {
            UIPlayerInfo.Add(new UI.UIPlayerInfo() { ControllerNum = 0, PlayerNum = 1, Skin = PlayerSkin.MoneyBoy, PlayerName = "Der Coole" });
            InitLevel();
        }

        public void InitLevel()
        {
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
                inited = true;
            }
        }

        private void Spawn()
        {
            System.Random rnd = new System.Random();
            SpawnPoints.OrderBy((item) => rnd.Next());

            int i = 0;
            UIPlayerInfo.ForEach((UIPlayerInfo PlayerInfo) =>
            {
                SpawnPlayer(PlayerInfo, SpawnPoints[i]);
                i++;
            });
        }

        private void SpawnPlayer(UIPlayerInfo PlayerInfo, Transform SpawnPoint)
        {
            GameObject ob = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation,null);
            PlayerManager PlayerManager = ob.GetComponent<PlayerManager>();
            PlayerInfo Info = PlayerManager.PInfo;
            Info.ControllerNum = PlayerInfo.ControllerNum;
            Info.PlayerNum = PlayerInfo.PlayerNum;
            PlayerInfo.Skin = Info.Skin;
            Info.Camera = cam;
        }

    }
}
