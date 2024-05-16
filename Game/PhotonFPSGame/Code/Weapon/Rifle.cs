using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rifle : MonoBehaviourPun
{
    [SerializeField] private GameObject _arm;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _bulletPref;
    [SerializeField] Transform _firePos;
    [SerializeField] private Transform _trsfWeaponPivot;
    [SerializeField] private Transform _trsfRHandMount;
    [SerializeField] private Transform _trsfLHandMount;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _shootingClip;
    // Magazine
    [SerializeField] private int _totalAmmo = 150; // 총알 개수
    [SerializeField] private int _currentAmmo; // 장전된 총알 개수
    [SerializeField] private int _reserveAmmo = 120; // 장전되지 않은 잔여 총알 개수
    [SerializeField] private int _magazineSize = 30; // 탄창 용량

    private bool _isReloading = false;

    public Transform TrsfWeaponPivot { get => _trsfWeaponPivot; }
    public Transform TrsfRHandMount { get => _trsfRHandMount; }
    public Transform TrsfLHandMount { get => _trsfLHandMount; }
    public int CurrentAmmo { get => _currentAmmo; }
    public bool IsReloading { get =>  _isReloading; }

    private void Start()
    {
        SetInit();

        if (!photonView.IsMine)
        {
            _arm.SetActive(false);
        }
    }

    public void Fire()
    {
        var bullet = PhotonNetwork.Instantiate(_bulletPref.name, _firePos.position, _firePos.rotation);

        _audio.PlayOneShot(_shootingClip);

        _currentAmmo -= 1;
        InGameUI.Instance?.UpdateAmmoDisplay(_currentAmmo, _reserveAmmo);

        //bullet.GetComponent<Rigidbody>().AddForce(_firePos.forward * 10f, ForceMode.Impulse);
    }

    public void Reload()
    {
        if (_currentAmmo < _magazineSize)
            StartCoroutine("ReloadWithDelay");
    }

    #region Private Methods

    private IEnumerator ReloadWithDelay()
    {
        _isReloading = true;
        _anim.SetTrigger("Reload");

        yield return new WaitForSeconds(3.3f);

        int ammo = _magazineSize - _currentAmmo;

        if(_reserveAmmo > ammo)
        {
            _currentAmmo = _magazineSize;
            _reserveAmmo -= ammo;
        }
        else
        {
            _currentAmmo += _reserveAmmo;
            _reserveAmmo = 0;
        }

        InGameUI.Instance?.UpdateAmmoDisplay(_currentAmmo, _reserveAmmo);

        _isReloading = false;
    }

    public void SetInit()
    {
        _reserveAmmo = _totalAmmo;
        _currentAmmo = _magazineSize;
        _reserveAmmo -= _magazineSize;
        InGameUI.Instance?.UpdateAmmoDisplay(_currentAmmo, _reserveAmmo);
    }

    #endregion
}
