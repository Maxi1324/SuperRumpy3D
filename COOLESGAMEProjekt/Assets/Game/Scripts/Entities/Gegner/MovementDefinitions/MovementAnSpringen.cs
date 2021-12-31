using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enemy.MovemetTransition;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.MovementDefinitions
{
    class MovementAnSpringen : IMovementDef
    {
        public NavMeshAgent Agent { get; set ; }

        public List<IMovementTransition> Transitions { get; private set; }
        public Gegner Geg { get ; set; }

        public Transform Target;

        public int AnlaufDis = 10;
        public Vector3 Sprungkraft;

        public MovementAnSpringen(int AnlaufDis, Vector3 Sprungkraft)
        {
            this.AnlaufDis = AnlaufDis;
            this.Sprungkraft = Sprungkraft;
        }

        public void Init()
        {
            Geg.Rb.isKinematic = true;
        }

        public void Stop()
        {
            Agent.enabled = true;
            Geg.Rb.isKinematic = true;
        }

        public void Update()
        {
            Vector3 dis = Agent.transform.position - Target.position;
            float length = dis.magnitude;
            Vector3 dir = dis.normalized;
            if(length > AnlaufDis)
            {
                Attack();
            }
            else
            {
                Vector3 pos = Agent.transform.position + (AnlaufDis - length) * dir;
                Agent.SetDestination(pos);
            }
        }

        public void Attack()
        {
            Agent.enabled = false;
            Geg.Rb.isKinematic = false;
            Geg.Rb.AddForce(Agent.transform.rotation*Sprungkraft);
        }
    }
}
