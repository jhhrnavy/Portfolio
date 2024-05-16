using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _anim;
    public const string MeleeAttack = "Right Stabbing";

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

}
