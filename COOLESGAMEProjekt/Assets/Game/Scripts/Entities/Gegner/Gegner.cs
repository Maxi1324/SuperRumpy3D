using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.MovementDefinitions;
using Enemy.MovemetTransition;
using UnityEngine.AI;
using System;
using Entity;

namespace Enemy
{
    public abstract class Gegner : Entity1
    {
        //TODO
        /*
         * --Movement Definition--
         * Damage Handler
         * Damage Hinzufüger
         * MovementChange Trigger
         * */

        public NavMeshAgent Agent;
        public Rigidbody Rb;
        public Animator Anim;

        private List<IMovementDef> Movements { get; set; }
        private int _aktMovement = 0;
        //TODO IndexOF ersetzen
        public IMovementDef AktMovement
        {
            get => Movements[_aktMovement];
            set => _aktMovement = (Movements.IndexOf(value));
        }

        private void Start()
        {
            if (Agent == null)
            {
                throw new Exception("Agent wurde nicht gesetzt");
            }
            if(Rb == null)
            {
                throw new Exception("Rigidbody wurde nicht gesetzt");
            }
            if(Anim == null)
            {
                throw new Exception("Anim wurde nicht gesetzt");
            }
            Rb.isKinematic = true;
            InitMovements();
            Start2();
        }

        public abstract void Start2();

        private void Reset()
        {
            Agent = GetComponent<NavMeshAgent>();
            Rb = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
        }

        public Gegner()
        {
            Movements = new List<IMovementDef>();
        }

        public abstract void Update2();

        private void Update()
        {
            Update2();
            AktMovement.Update();
        }

        public void CheckForTransition()
        {
            foreach(IMovementTransition Transition in AktMovement.Transitions)
            {
                if (Transition.Transition())
                {
                    ChangeMovement(Transition.Movement);
                }
            }
        }

        public bool AddMovement(IMovementDef Movement)
        {
            foreach(IMovementDef movement in Movements)
            {
                if(movement.GetType() == Movement.GetType())
                {
                    Movement = null;
                }
            }
            if(Movement != null)
            {
                Movements.Add(Movement);
                Movement.Agent = Agent;
                Movement.Geg = this;
                return true;
            }
            return false;
        }

        public void ChangeMovement(IMovementDef Movement)
        {
            AktMovement?.Stop();
            AktMovement = Movement;
            AktMovement.Init();
        }

        public abstract void InitMovements();
    }
}