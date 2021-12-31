using System;
using System.Collections.Generic;
using Enemy.MovemetTransition;
using UnityEngine.AI;
using UnityEngine;

namespace Enemy.MovementDefinitions
{
    public class MovementCircle : IMovementDef
    {
        public NavMeshAgent Agent { get ; set ; }
        public List<IMovementTransition> Transitions { get; set; }

        public float Grad { get; private set; }
        public float Speed { get; set; }
        public float Radius { get; set; }
        public Transform Target { get; set; }
        public Gegner Geg { get ; set ; }

        public void Init()
        {
            Geg.Rb.isKinematic = true;
            Geg.Agent.enabled = true;
            Vector3 Dis = Agent.transform.position - Target.position;
            if(Radius == 0)Radius = Dis.magnitude;
            Grad = Mathf.Acos(Dis.normalized.x);
        }

        public void Stop()
        {
            Geg.Agent.enabled = true;
            Geg.Rb.isKinematic = true;
        }

        public void Update()
        {
            Grad += Speed * Time.deltaTime;
            Vector3 Goal = new Vector3(Mathf.Cos(Grad), 0, Mathf.Sin(Grad))*Radius+Target.position;
            Agent.SetDestination(Goal);
            Debug.DrawLine(Goal, Goal + Vector3.up * 100000, Color.green);
        }
    }
}
