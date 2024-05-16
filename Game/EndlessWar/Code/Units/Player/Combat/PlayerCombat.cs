using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCombat : MonoBehaviour
{
    public virtual void Attack() { }

    public virtual void CancledAttack() { }
}

public enum AttackType
{
    None = -1,
    Shooting,
    Melee,
    Throwing
}
