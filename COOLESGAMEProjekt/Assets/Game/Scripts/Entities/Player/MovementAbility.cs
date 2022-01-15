using Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Needed for new Movement Abillities
/// </summary>
/// <remarks>
/// Schritte die zu tun sind:
/// Definieren, ob es ein InteractOB brauchts(in start)
/// Definieren, ob das Movement gerade gemacht werden kann(Allowedmoves checken)
/// Definieren der Active Methode.
/// </remarks>
/// 
namespace Entity.Player.Abilities
{
    public abstract class MovementAbility : MonoBehaviour
    {
        /// <summary>
        /// Entscheidet, ob ein InteractOB gebraucht wird.
        /// </summary>
        public bool NeedObject { get; set; }

        /// <summary>
        /// Definiert, was jetzt genau gemacht werden soll.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="ob"></param>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public abstract bool Active(float distance, InteractPlayer ob, bool allowed);

        /// <summary>
        /// Definiert, ob das hier alles passieren darf. Muss selber genutzt werden, lol^123
        /// </summary>
        /// <param name="allowedMoves"></param>
        /// <returns></returns>
        public abstract bool Allowed(int allowedMoves);

        public virtual void HelperFunction()
        {

        }

        public virtual void HelperFunction2()
        {

        }
    }
}