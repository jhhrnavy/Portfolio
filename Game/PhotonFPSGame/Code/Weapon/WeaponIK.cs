using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private Rifle _rifle;

    //[SerializeField] Transform _lookAtTarget;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_anim == null) return;

        _rifle.TrsfWeaponPivot.position = _anim.GetIKHintPosition(AvatarIKHint.RightElbow);

        //_anim.SetLookAtWeight(1);
        //_anim.SetLookAtPosition(_lookAtTarget.position);

        // Righthand IK setting
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _rifle.TrsfRHandMount.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _rifle.TrsfRHandMount.rotation);

        // Lefthand IK setting
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, _rifle.TrsfLHandMount.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, _rifle.TrsfLHandMount.rotation);
    }
}
