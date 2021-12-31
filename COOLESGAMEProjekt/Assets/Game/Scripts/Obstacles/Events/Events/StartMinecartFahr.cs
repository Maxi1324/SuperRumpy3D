using Entity.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles.Events
{

    public class StartMinecartFahr : MonoBehaviour
    {
        public GameObject LorenPrefab;

        public void StartMineCart()
        {
            return;
            PlayerManager[] Players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager Player in Players)
            {
                Player.gameObject.SetActive(false);
                Player.transform.parent = null;
                GameObject ob = Instantiate(LorenPrefab, Player.transform.position, Player.transform.rotation);
                PlayerInfo info = ob.GetComponent<PlayerInfo>();
                Player.PInfo.CloneInfo(info);
            }
        }
    }
}