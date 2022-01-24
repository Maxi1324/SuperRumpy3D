using Entity.Player;
using Entity.Player.Abilities;
using System;
using UnityEngine;

namespace Env
{
    public class AbillityFreischalten:MonoBehaviour
    {
        public int Ab;
        public GameObject ob;
        private bool einmal = false;

        private void OnCollisionEnter(Collision collision)
        {
            if(einmal)
            {
                return;
            }
            if(collision.gameObject.tag == "Player")
            {
                Destroy(ob);
                PlayerPrefs.SetInt(("Ab" + Ab), 1);

                PlayerManager[] PMs = FindObjectsOfType<PlayerManager>();
                foreach(PlayerManager PM in PMs)
                {
                    MovementAbility Abilit = null;
                    switch (Ab)
                    {
                        case 0:
                            Abilit = (MovementAbility)PM.GetComponentInChildren<GrapplingHook>(true);
                            break;
                        case 1:
                            Abilit = (MovementAbility)PM.GetComponentInChildren<LongJump>(true);
                            break;
                        case 2:
                            Abilit = (MovementAbility)PM.GetComponentInChildren<HighJump>(true);
                            break;
                    }
                    Abilit.enabled = true;
                    PM.Moves.Add(Abilit);
                    einmal = true;
                }
            }
        }
    }
}
