using System.Collections;
using UnityEngine;

public class PlayerMeleeAttack : PlayerCombat
{
    [SerializeField] public NewSword weapon;
    private Animator _anim;

    [SerializeField] private bool _isAttacking = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if(weapon != null && !_isAttacking)
        {
            _isAttacking = true;
            _anim.SetTrigger("Knife Attack");
        }
    }

    #region Animation Event Method

    public void AnimEventHitTarget()
    {
        if (weapon.HitBox.IsHit())
            weapon.HitBox.GetDetectedCollider().GetComponent<Enemy>().GetHit(weapon.damage);
    }

    public void AnimEventEndAttack()
    {
        _isAttacking = false;
    }

    #endregion

    #region Unused

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        bool isHit = false;
        _anim.SetTrigger("Knife Attack");
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(1);

        // is animation started
        while (!stateInfo.IsName("Right Stabbing"))
        {
            yield return null;
            stateInfo = _anim.GetCurrentAnimatorStateInfo(1);
        }

        // is animation finished?
        while (stateInfo.IsName("Right Stabbing"))
        {
            yield return null;

            stateInfo = _anim.GetCurrentAnimatorStateInfo(1);

            // 적이 범위내에 있으면 해당적 공격 ( 단일 공격)
            if (!isHit && stateInfo.normalizedTime > 0.2f && weapon.HitBox.IsHit())
            {
                weapon.HitBox.GetDetectedCollider().GetComponent<Enemy>().GetHit(weapon.damage);
                isHit = true;
            }
        }

        _isAttacking = false;

    }

    #endregion
}
