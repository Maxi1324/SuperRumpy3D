using Enemy.MovementDefinitions;
using UnityEngine;

namespace Enemy.MovemetTransition
{
    public interface IMovementTransition
    {
         GameObject gameObject { get; set; }
         IMovementDef Movement { get; set; }
         bool Transition();
    }
}
