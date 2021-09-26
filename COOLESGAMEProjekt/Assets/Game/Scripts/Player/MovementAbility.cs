using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbility : MonoBehaviour
{
    public abstract bool Active(float distance,InteractPlayer ob, bool allowed);

    public abstract bool Allowed(int allowedMoves);
}
