using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSword : NewWeapon
{
    [SerializeField] private HitBox _hitBox;
    public int damage;

    private void Start()
    {
        transform.localPosition = localPosition;
    }

    public HitBox HitBox { get => _hitBox;}
}
