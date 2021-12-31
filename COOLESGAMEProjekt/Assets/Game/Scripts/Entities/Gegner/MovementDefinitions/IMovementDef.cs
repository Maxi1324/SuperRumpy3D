using UnityEngine.AI;
using Enemy.MovemetTransition;
using System.Collections.Generic;

namespace Enemy.MovementDefinitions
{
    public interface IMovementDef
    {
        NavMeshAgent Agent { get; set; }
        Gegner Geg { get; set; }
        List<IMovementTransition> Transitions { get; }
        void Init();
        void Update();
        void Stop();
    }
}
