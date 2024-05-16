using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New MeleeWeapon", menuName = "Weapon/Melee")]
public class Knife : Weapon
{
    //[SerializeField] private HitBox _hitBox;

    //[SerializeField]
    //private Animator _anim;

    //public int damage = 10;
    //public float attackSpeed;


    //private PlayerInputActions _controls;

    //private void Awake()
    //{
    //    _controls = new PlayerInputActions();

    //    _controls.GamePlay.MeleeAttack.started += context => StartAttack();
    //}
    //private void OnEnable()
    //{
    //    _controls.Enable();
    //}

    //private void OnDisable()
    //{
    //    _controls.Disable();
    //}

    //private void Update()
    //{
    //    AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(1);

    //    if (stateInfo.IsName("Right Stabbing") && stateInfo.normalizedTime >= 0.8f)
    //    {
    //        EndAttack();
    //    }
    //}
    //public void StartAttack()
    //{
    //    if (isAttacking) return;
    //    isAttacking = true;
    //    _anim.SetTrigger("Knife Attack");
    //    Invoke("PerformedAttack", 0.4f);
    //}
    //public void PerformedAttack()
    //{
    //    if (_hitBox.IsHit())
    //    {
    //        _hitBox.GetDetectedCollider().gameObject.GetComponent<Enemy>().GetHit(damage);
    //    }
    //}
    //public void EndAttack()
    //{
    //    isAttacking = false;
    //}

}
