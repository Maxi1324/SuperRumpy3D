using System;
using UnityEngine;

namespace Entity
{
    public abstract class Entity1 : MonoBehaviour
    {
        public abstract IDamageHandler DamageHandler { get; }
    }
}