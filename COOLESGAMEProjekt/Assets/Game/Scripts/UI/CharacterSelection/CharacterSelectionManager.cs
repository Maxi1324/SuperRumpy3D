using System.Collections.Generic;
using UnityEngine;
using UI.CharacterSelection.CharacterRegistrierer;
using System;
using Generell;
using System.Diagnostics;

namespace UI.CharacterSelection
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        public List<ICharacterRegistrierer> Registrierer = new List<ICharacterRegistrierer>();
        public List<UIPlayerInfo> Players = new List<UIPlayerInfo>();
        public List<PlayerUIOB> PlayerUIObs = new List<PlayerUIOB>();
        public GameObject PressA;

        void Start()
        {
            foreach (ICharacterRegistrierer CR in GetComponents<ICharacterRegistrierer>())
            {
                if (!Registrierer.Contains(CR))
                {
                    Registrierer.Add(CR);
                }
            }
        }

        void Update()
        {
            Registrierer.ForEach(CR =>
            {
                CR.FindNewPlayers().ForEach(Info =>
                {
                    AddPlayer(Info);
                });
            });
            if(Players.Count >= 1)
            {
                PressA.SetActive(true);
            }

            foreach (UIPlayerInfo Player in Players)
            {
                if (Input.GetButtonDown("Fire2" + Player.ControllerNum) &&  (Stopwatch.GetTimestamp() - Player.RegisZeit) > 30000)
                {
                    SceneLoader.loadScene("BergLevel",()=>{

                    });
                }
            }
        }

        private void AddPlayer(UIPlayerInfo Player)
        {
            if (Players.Contains(Player))
            {
                throw new Exception("Player exsestiert bereits");
            }
            Players.Add(Player);
            PlayerUIOB PUIOB = PlayerUIObs[Player.PlayerNum];
            PUIOB.Player = Player;
        }
    }
}