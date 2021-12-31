using System;
using UnityEngine;
using Enemy.MovementDefinitions;

namespace Enemy.MovemetTransition
{
    public class TransitionToNear:IMovementTransition
    {
        public float dis { get; set; }
        public IMovementDef Movement { get ; set ; }
        public GameObject gameObject { get ; set ; }

        public TransitionToNear(float dis, IMovementDef TransitionTo)
        {
            this.dis = dis;
            Movement = TransitionTo;
        }

        public bool Transition()
        {
            //PlayerInfo[] Players = 
            return true;
        }
    }
}
