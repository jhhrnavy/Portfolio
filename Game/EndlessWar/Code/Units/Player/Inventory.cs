using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _size;
    // 0 = primary, 1 = secondary, 2 = melee, 3 = Grenade
    [SerializeField] private NewWeapon[] _weapons = new NewWeapon[4];

    public int Size { get => _size;}

    private void Start()
    {
        _size = _weapons.Length;
    }

    public void AddItem(NewWeapon newWeapon)
    {
        _weapons[(int)newWeapon.weaponStyle] = newWeapon;
    }

    public void RemoveItem(int index)
    {
        _weapons[index] = null;
    }

    public NewWeapon GetItem(int index)
    {
        return _weapons[index];
    }

}
