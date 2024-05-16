using UnityEngine;

public class Weapon : ScriptableObject
{
    public string name;
    public GameObject prefab;
    public WeaponType weaponType;
    public WeaponStyle weaponStyle;

    public int damage;
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public virtual void Fire(Vector3 firePosition, Vector3 targetPosition) { }

    public virtual void MeleeAttack() { }
}

public enum WeaponType
{
    Melee,
    Pistol,
    AR,
    Bomb
}

public enum WeaponStyle
{
    None = -1,
    Primary,
    Secondary,
    Melee,
    Throwing
}
