using UnityEngine;
using Entity.Player;
using Obstacles;
using Obstacles.MoveableBlocks;

namespace Assets.Game.Scripts.Entities.Player.AdvancedMovement
{
    public class MoveBlocks : MovementAbility
    {
        public PlayerManager Player;
        public float MinDis = 1;


        private void Reset()
        {
            Player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            NeedObject = true;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (allowed)
            {
                if (ob.key == "Block" && distance <= MinDis  && Player.Fire3Pressed)
                {
                    Debug.Log("alasofpdiüjlowed");

                    MoveableBlock MB =  ob.GetComponent<MoveableBlock>();
                    MB.Move(transform);
                    Player.AllowedMoves = -1;
                    StartCoroutine(Player.DoInNSec(()=>
                    {
                        Player.AllowedMoves = 0;
                    },1));
                    return true;
                }
            }
            return false;
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0 || allowedMoves == 1;
        }
    }
}
