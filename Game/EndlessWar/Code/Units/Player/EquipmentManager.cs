using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    private Inventory _inventory;
    private WeaponIKController _weaponIKController;
    private PlayerShooting _playerShooting;
    private PlayerMeleeAttack _playerMeleeAttack;

    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private GameObject[] _equipments = new GameObject[4];
    [SerializeField] private NewWeapon _equipedWeapon;

    private Rigidbody _curWeaponRb;
    private Collider _curWeaponCollider;
    private float dropForwardForce = 3f;
    private float dropUpwardForce = 3f;

    public NewWeapon EquipedWeapon { get => _equipedWeapon; }
    public int AttackType { get; private set; }

    private void Start()
    {
        _weaponIKController = GetComponent<WeaponIKController>();
        _inventory = GetComponent<Inventory>();
        _playerShooting = GetComponent<PlayerShooting>();
        _playerMeleeAttack = GetComponent<PlayerMeleeAttack>();

        // Set Init Weapon , melee
        SwitchWeapon((int)WeaponStyle.Melee);
    }

    #region Input Event Function

    public void OnSwitchWeapons(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int index = (int)context.ReadValue<float>() - 1;
            SwitchWeapon(index);
        }
    }

    public void OnDropWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed || 
            _equipedWeapon == null ||
            _equipedWeapon.weaponStyle == WeaponStyle.Melee ||
            _equipedWeapon.weaponStyle == WeaponStyle.Throwing) return;

        int dropWeaponIndex = (int)_equipedWeapon.weaponStyle;
        Debug.Log("호출");
        Drop();

        // 인벤토리에서 무기 제거
        _inventory.RemoveItem(dropWeaponIndex);

        // Unequipped
        UpdateEquipmentInfo(null, null, null);

        // 애니메이션 IK 변경
        _weaponIKController.SetInit();

        SwitchWeapon(dropWeaponIndex + 1);
    }

    #endregion

    #region public Method

    public void EquipWeapon(int weaponStyle)
    {
        if (_equipedWeapon != null)
            _equipedWeapon.gameObject.SetActive(false);

        if (_equipments[weaponStyle] != null)
        {
            _equipments[weaponStyle].SetActive(true);
            _equipedWeapon = _equipments[weaponStyle].GetComponent<NewWeapon>();
            UpdateEquipmentInfo(_equipments[weaponStyle], _equipments[weaponStyle].GetComponent<Rigidbody>(), _equipments[weaponStyle].GetComponent<Collider>());
        }

        if (_curWeaponRb != null)
            _curWeaponRb.isKinematic = true;
        if (_curWeaponCollider != null)
            _curWeaponCollider.isTrigger = true;

        // 애니메이션 IK 변경
        _weaponIKController.ChangeWeaponIK(_equipedWeapon.transform, _equipedWeapon.trsfRHandMount, _equipedWeapon.trsfLHandMount);

        ChangeAttackMode();
    }

    public void AddEquipment(GameObject equipment)
    {
        equipment.transform.SetParent(_weaponHolder);
        var weaponInfo = equipment.GetComponent<NewWeapon>();
        equipment.transform.localPosition = weaponInfo.localPosition;
        equipment.transform.localRotation = Quaternion.Euler(weaponInfo.localRotation);
        equipment.transform.localScale = weaponInfo.localScale;

        _equipments[(int)weaponInfo.weaponStyle] = equipment;
    }

    #endregion

    #region Private Method

    private void Drop()
    {
        // Physics Drop weapon
        _equipedWeapon.transform.SetParent(null);
        _curWeaponRb.isKinematic = false;
        _curWeaponCollider.isTrigger = false;

        // Throwing force
        _curWeaponRb.velocity = GetComponent<Rigidbody>().velocity;
        _curWeaponRb.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
        _curWeaponRb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);

        _equipedWeapon.gameObject.layer = 10;
        _equipments[(int)_equipedWeapon.weaponStyle] = null;
    }

    private void UpdateEquipmentInfo(GameObject weapon, Rigidbody rb, Collider coll)
    {
        if(weapon != null)
            _equipedWeapon = weapon.GetComponent<NewWeapon>();
        else
            _equipedWeapon = null;

        _curWeaponRb = rb;
        _curWeaponCollider = coll;
    }

    private void ChangeAttackMode()
    {
        switch (_equipedWeapon.weaponStyle)
        {
            case WeaponStyle.None:
                break;

            case WeaponStyle.Primary:
            case WeaponStyle.Secondary:
                _playerShooting.SetGun(_equipedWeapon);
                AttackType = 0;
                break;

            case WeaponStyle.Melee:
                _playerMeleeAttack.weapon = _equipedWeapon as NewSword;
                AttackType = 1;
                break;

            case WeaponStyle.Throwing:
                AttackType = 2;
                break;
        }
    }

    private void SwitchWeapon(int weaponStyle)
    {
        if (_equipments[weaponStyle] != null)
            EquipWeapon(weaponStyle);
    }

    #endregion
}
