using System;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public enum Owner
    {
        Player,
        Enemy
    }

    public Owner owner;

    private PlayerInputActions _controls;

    public int damage;

    public int currentAmmo; // 현재 장전된 총알 개수
    public int magazineSize; // 탄창 크기
    public int reserveAmmo; // 남아있는 총알

    public float fireRate, spread;
    public float reloadTime = 0.5f;
    private bool _readyToFire, _isFiring, _reloading;
    public bool allowsAutoShot = true;

    private Vector3 targetPosition;

    [SerializeField]
    private GameObject _bulletPref;

    [SerializeField]
    private Transform _firePos;

    [SerializeField]
    private float _bulletSpeed;


    // Weapon Handle Data
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public bool IsReloading { get => _reloading; }

    public static event Action<int, int, int> OnAmmoChanged;

    //private void Awake()
    //{
    //    if (gameObject.GetComponentInParent<PlayerController>())
    //    {
    //        owner = Owner.Player;
    //        _controls = new PlayerInputActions();
    //        _controls.GamePlay.Fire.started += context => StartFiring();
    //        _controls.GamePlay.Fire.canceled += context => EndFiring();
    //        _controls.GamePlay.Reload.performed += context => Reload();

    //        OnAmmoChanged?.Invoke(currentAmmo, magazineSize, reserveAmmo); // UI Update event call
    //    }
    //}

    //private void Start()
    //{
    //    _readyToFire = true;
    //}

    //private void OnEnable()
    //{
    //    if (owner == Owner.Player)
    //        _controls.Enable();
    //}

    //private void OnDisable()
    //{
    //    if (owner == Owner.Player)
    //        _controls.Disable();
    //}

    //private void Update()
    //{
    //    if (_readyToFire && _isFiring && !_reloading && currentAmmo > 0)
    //    {
    //        if (owner == Owner.Player)
    //            PerformFiring(GetMouseHitPosition());
    //        else if (owner == Owner.Enemy)
    //            PerformFiring(targetPosition);
    //    }
    //}

    public void SetPosition()
    {
        transform.localPosition = localPosition;
    }

    public void StartFiring()
    {
        _isFiring = true;
    }

    private void PerformFiring(Vector3 target)
    {
        _readyToFire = false;

        // Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        Vector3 direction = target - transform.position + new Vector3(x, y, 0);
        direction.y = 0;

        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * _bulletSpeed, ForceMode.Impulse);

        currentAmmo--;

        if (owner == Owner.Player)
            OnAmmoChanged?.Invoke(currentAmmo, magazineSize, reserveAmmo); // UI Update event call

        if (currentAmmo > 0)
        {
            Invoke("ResetFiring", fireRate);
        }

        // 단발 사격
        if (!allowsAutoShot)
        {
            EndFiring();
        }
    }

    public void EndFiring()
    {
        _isFiring = false;
    }

    public void ResetFiring()
    {
        Debug.Log("call invoke");

        _readyToFire = true;
    }

    public void Reload()
    {
        _reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    public void ReloadFinish()
    {
        int temp = (magazineSize - currentAmmo);

        if (temp < reserveAmmo)
        {
            currentAmmo += temp;
            reserveAmmo -= temp;
        }
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }

        if (owner == Owner.Player)
            OnAmmoChanged?.Invoke(currentAmmo, magazineSize, reserveAmmo); // UI Update event call

        _reloading = false;
        ResetFiring();
    }


    private Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
        }

        return mousePosition;
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }
}
