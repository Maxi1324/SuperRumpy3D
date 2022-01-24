using System;
using System.Collections.Generic;
using Obstacles;
using Obstacles.Schild;
using UI.InGameUi;
using UnityEngine;
namespace Entity.Player.Abilities
{
    public class LookAtSign : MovementAbility
    {
        public float maxDis = 15;
        public PlayerManager PM;

        private bool hallo;
        private GameObject Tooltip;

        private void Reset()
        {
            PM = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            if (PM == null)
            {
                PM = GetComponent<PlayerManager>();
            }
            NeedObject = true;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed)
            {
                if (ob.key == "Schild")
                {
                    if (distance < maxDis)
                    {
                        if (Tooltip == null&&!hallo)
                        {
                            Tooltip = WorldSpaceUiFunctions.Instance.ShowTooltip(UI.InGameUi.Tooltip.A, PM.PInfo.PHandTransform.position);
                            StartCoroutine(WorldSpaceUiFunctions.Instance.FollowTransform(PM.PInfo.PHandTransform, Tooltip.transform));
                            hallo = true;
                        }
                        if (PM.Fire3Pressed)
                        {
                            Schild schild = ob.GetComponent<Schild>();
                            PM.AllowedMoves = -1;   
                            schild.show(() =>
                            {
                                PM.AllowedMoves = 0;
                                hallo = true;
                            });
                            return true;
                        }
                    }
                    else
                    {
                        if (Tooltip != null)
                        {
                            WorldSpaceUiFunctions.Instance.DestroyTooltip(Tooltip,()=> { });
                            Tooltip = null;
                        }
                    }
                }
            }
            return false;
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0;
        }
    }
}
