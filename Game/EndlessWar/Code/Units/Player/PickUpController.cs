using System;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private Inventory _inventory;
    private EquipmentManager _equipmentManager;
    [SerializeField] private float _pickUpRadius = 2;
    [SerializeField] private LayerMask _weaponMask;
    private const int WEAPON_LAYER = 9;
    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        _equipmentManager = GetComponent<EquipmentManager>();
    }

    private void Update()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _pickUpRadius, _weaponMask);

        foreach(Collider coll in colls)
        {
            if(coll != null)
                PickUp(coll.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _pickUpRadius);
    }

    public void PickUp(GameObject weapon)
    {
        // TODO: 캐스팅 문제 없나?
        var newWeapon = weapon.GetComponent<NewWeapon>();

        if (_inventory.GetItem((int)newWeapon.weaponStyle) == null)
        {
            weapon.layer = WEAPON_LAYER;
            _inventory.AddItem(newWeapon);
            // weaponHolder에 넣기
            _equipmentManager.AddEquipment(weapon);
            weapon.SetActive(false);
        }

    }
}
