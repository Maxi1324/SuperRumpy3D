using Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.CharacterSelection;
using UnityEngine;

namespace Generell.LevelManager1
{
    public class DieManager : MonoBehaviour
    {
        private List<PlayerManager> Dead { get; set; } = new List<PlayerManager>();
        private LevelManager Manager { get; set; }

        public bool informed = false;

        private void Start()
        {
            if (Manager == null)
            {
                Manager = FindObjectOfType<LevelManager>();
            }
        }

        private void Update()
        {
             PlayerManager[] Players = FindObjectsOfType<PlayerManager>();

            foreach (PlayerManager Player in Players)
            {
                if (Player.aktLeben <= 0)
                {
                    if (!Dead.Contains(Player))
                    {
                        Dead.Add(Player);
                    }
                }
            }
            for (int i = 0; i < Dead.Count; i++)
            {
                PlayerManager player = Dead[i];
                if (player.aktLeben > 0)
                {
                    Dead.Remove(player);
                    i--;
                }
            }

            if (Dead.Count == CharacterSelectionManager.Players.Count)
            {  
                if (!informed)
                {
                    informed = true;
                    Manager.InitLevel();
                    Dead.Clear();
                }
            }
        }

        public void Respawn(List<Transform> SpawnPoints)
        {
            System.Random rnd = new System.Random();
            SpawnPoints.OrderBy((item) => rnd.Next());
            int i = 0;
            Dead.ForEach(p =>
            {
                p.gameObject.SetActive(true);
                p.Spawn(SpawnPoints[i].position);
                i++;
            });
        }
    }
}