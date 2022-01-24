using System.Collections.Generic;
using UnityEngine;
using UI.CharacterSelection.CharacterRegistrierer;
using System;
using Generell;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace UI.CharacterSelection
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        public List<ICharacterRegistrierer> Registrierer = new List<ICharacterRegistrierer>();
        public static List<UIPlayerInfo> Players = new List<UIPlayerInfo>();
        public List<PlayerUIOB> PlayerUIObs = new List<PlayerUIOB>();
        public GameObject PressA;

        public GameObject needController;

        public List<Tuple<PlayerSkin, bool>> FreeSkins = new List<Tuple<PlayerSkin, bool>>();

        void Start()
        {
            Players.Clear();
            for (int i = 0; i < 4; i++)
            {
                FreeSkins.Add(new Tuple<PlayerSkin, bool>((PlayerSkin)i, true));
            }

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
            needController.SetActive(Input.GetJoystickNames().Length == 0);
            foreach (UIPlayerInfo Player in Players)
            {
                if (Input.GetButtonDown("Jump" + Player.ControllerNum))
                {
                    SceneLoader.loadScene(PlayerPrefs.GetInt("FirstTime") == 1?"LevelSelection": "StartScene", ()=>{

                    });
                }
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    PlayerPrefs.SetInt("Level", 5);
                }

                if (Input.GetKeyDown(KeyCode.U))
                {
                    PlayerPrefs.DeleteAll();
                    Debug.Log("HAAALO");
                }
                if (Player.Editable)
                {
                    float x = Input.GetAxis("Horizontal" + Player.ControllerNum);
                    if (Math.Abs(x) > 0.8f && !Player.WasUpStick)
                    {
                        Player.WasUpStick = true;
                        int dir = x > 0 ? 1 : -1;
                        changeSkin(Player, dir);
                    }
                    if(Math.Abs(x) < 0.3f) {
                        Player.WasUpStick = false;
                    }
                }
            }

            Registrierer.ForEach(CR =>
            {
                CR.FindNewPlayers().ForEach(Info =>
                {
                    AddPlayer(Info);
                });
            });
            if (Players.Count >= 1)
            {
                PressA.SetActive(true);
            }

            if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                ResetPlayerPrefs();
            }
        }

        public void ResetPlayerPrefs()
        {
            Debug.Log("Playerprefs");
            PlayerPrefs.SetInt("FirstTime", 1);
        }

        public void changeSkin(UIPlayerInfo Player, int dir)
        {
            int lastPos = (int)Player.Skin;
            for (bool Found = false; !Found;)
            {

                int Index = lastPos + dir;
                if (Index > 3) Index = 0;
                if (Index < 0) Index = 3;
                lastPos = Index;

                if (lastPos == (int)Player.Skin)
                {
                    break;
                }

                Tuple<PlayerSkin, bool> ToCheck = FreeSkins[Index];
                if (ToCheck.Item2)
                {
                    FreeSkins[(int)Player.Skin] = new Tuple<PlayerSkin, bool>(FreeSkins[(int)Player.Skin].Item1, true);
                    FreeSkins[Index] = new Tuple<PlayerSkin, bool>(ToCheck.Item1, false);
                    Player.Skin = ToCheck.Item1;
                    Found = true;
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
            changeSkin(Player, 1);
            PlayerUIOB PUIOB = PlayerUIObs[Player.PlayerNum];
            PUIOB.Player = Player;
        }
    }
}