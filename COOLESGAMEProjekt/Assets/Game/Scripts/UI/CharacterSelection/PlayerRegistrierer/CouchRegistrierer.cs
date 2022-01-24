using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.CharacterSelection.CharacterRegistrierer
{
    class CouchRegistrierer : MonoBehaviour, ICharacterRegistrierer
    {
        public string Button = "Jump";
        private List<int> Players = new List<int>();

        public List<UIPlayerInfo> FindNewPlayers()
        {
            List<UIPlayerInfo> PlayerInfos = new List<UIPlayerInfo>();
            for(int i = 0; i < 4; i++)
            {
                if (!Players.Contains(i))
                {
                    if (Input.GetButtonDown(Button + (i+1)))
                    {
                        UIPlayerInfo Info = new UIPlayerInfo() {ControllerNum = (i+1), PlayerName = "Player"+Players.Count,PlayerNum = Players.Count, Skin = (PlayerSkin)3 };
                        Players.Add(i);
                        PlayerInfos.Add(Info);
                    }
                }
            }
            return PlayerInfos;
        }
    }
}
