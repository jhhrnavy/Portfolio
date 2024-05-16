using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewWeapon : MonoBehaviour
{
    public string name;
    public WeaponType weaponType;
    public WeaponStyle weaponStyle;

    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    [Header("Anim IK"), Space]
    public Transform trsfRHandMount;
    public Transform trsfLHandMount;

}
