using UnityEngine;

public class WeaponIKController : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private Transform _trsfWeaponPivot;
    [SerializeField] private Transform _trsfRHandMount;
    [SerializeField] private Transform _trsfLHandMount;
    public Transform currentWeapon;

    private float _rightHandIkWeightValue = 0f;
    private float _leftHandIkWeightValue = 0f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(1);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (_anim == null || currentWeapon == null) return;

        _trsfWeaponPivot.position = _anim.GetIKHintPosition(AvatarIKHint.RightElbow);

        // Righthand IK setting
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandIkWeightValue);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightHandIkWeightValue);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _trsfRHandMount.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _trsfRHandMount.rotation);

        // Lefthand IK setting
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandIkWeightValue);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftHandIkWeightValue);
        if (_trsfLHandMount != null)
        {
            _anim.SetIKPosition(AvatarIKGoal.LeftHand, _trsfLHandMount.position);
            _anim.SetIKRotation(AvatarIKGoal.LeftHand, _trsfLHandMount.rotation);
        }
    }

    public void ChangeWeaponIK(Transform newWeapon, Transform trsfRHandMount, Transform trsfLHandMount)
    {
        currentWeapon = newWeapon;
        _trsfRHandMount = trsfRHandMount;
        _trsfLHandMount = trsfLHandMount;

        _rightHandIkWeightValue = 1f;
        if (_trsfLHandMount != null)
            _leftHandIkWeightValue = 1f;
        else
            _leftHandIkWeightValue = 0f;

        // 상체 모션 변경
        if (newWeapon.GetComponent<NewWeapon>() is NewSword)
        {
            _anim.SetLayerWeight(1, 1f);
            _anim.SetLayerWeight(2, 0f);

        }
        else if (newWeapon.GetComponent<NewWeapon>() is Bomb)
        {
            _anim.SetLayerWeight(2, 1f);
            _anim.SetLayerWeight(1, 0f);
        }
        else
        {
            _anim.SetLayerWeight(1, 0f);
            _anim.SetLayerWeight(2, 0f);
        }
    }

    public void SetInit()
    {
        currentWeapon = null;
        _trsfRHandMount = null;
        _trsfLHandMount = null;
    }

}
