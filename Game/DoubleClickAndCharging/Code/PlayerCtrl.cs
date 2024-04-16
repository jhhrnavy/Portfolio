using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region Reference

    [SerializeField] GameObject[] _weaponPrefs;
    [SerializeField] Laser _laser;
    [SerializeField] Transform _firePos;
    [SerializeField] GameObject _expEffect;

    #endregion

    #region Field

    [Header("[ �ִ� ü�� ]"), SerializeField]
    private int _maxHp = 10;

    [Header("[ ü�� ]"), SerializeField]
    private int _hp;

    [Header("[ �̵� �ӵ� ]"), SerializeField]
    private float _moveSpeed;
    [Header("[ ȸ�� �ӵ� ]"), SerializeField]
    private float _rotSpeed;

    // Horizontal Input
    private float _h;

    private int _destroyEnemyCount = 0;

    [Header("���� Ŭ�� ���� �ð�"), SerializeField]
    private float _doubleClickTime = 0.15f;
    private bool _isSingleClick = false;
    private double _timer = 0;

    [Header("�� Ŭ�� ���� �ð�"), SerializeField]
    private float _longClickTime = 2f;

    [Header("��ź ��Ÿ��"), SerializeField]
    private float _bombCoolTime = 5f;
    [Header("������ ��Ÿ��"), SerializeField]
    private float _laserCoolTime = 5f;

    //-----------------------------------------------
    private float _totalChargingTimer = 0;
    private float _bombCoolTimer;
    private float _laserCoolTimer;
    private float _hitDelay;
    private bool _isLongClickLaunch = false; // Charging Complete : true
    private bool _fireBombFlag = true;  // ��Ÿ���� ���ƿԳ���?
    private bool _fireLaserFlag = true;
    private bool _isDead = false;

    #endregion

    #region Properties

    public int DestroyEnemyCount { get => _destroyEnemyCount; set => _destroyEnemyCount = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public float BombCoolTime { get => _bombCoolTime; }
    public float LaserCoolTime { get => _laserCoolTime; }
    public float BombCoolTimer { get => _bombCoolTimer; }
    public float LaserCoolTimer { get => _laserCoolTimer; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    #endregion

    #region Start And Update

    // Start is called before the first frame update
    private void Start()
    {
        _h = Input.GetAxis("Horizontal");

        _hp = _maxHp;
        UIManager.Instance.SetHpDisplay(_maxHp);

        _bombCoolTimer = _bombCoolTime;
        UIManager.Instance.SetBombGageDisplay(_bombCoolTime);

        _laserCoolTimer = _laserCoolTime;
        UIManager.Instance.SetLaserGageDisplay(_laserCoolTime);
    }

    // Update is called once per frame
    private void Update()
    {
        _h = Input.GetAxis("Horizontal");

        if (!_laser.IsFireLaser) Move();

        CheckFireBigBombConditions();
        CheckFireFireLaserConditions();

        // MouseButton(0)
        OnFireBomb();
        // MouseButton(1)
        OnFireLaser();
    }

    #endregion

    #region Input Action Control

    private void OnFireBomb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isSingleClick)
            {
                _timer = Time.time;
                _isSingleClick = true;
            }
            else
            {
                // ���콺 �¹�ư ����Ŭ���� ���� ��ź �߻�
                if ((Time.time - _timer) < _doubleClickTime)
                {
                    if (_fireBombFlag)
                    {
                        FireBomb(1);
                        _bombCoolTimer = _bombCoolTime;
                        UIManager.Instance.UpdateBombGageDisplay(_bombCoolTimer);
                        _fireBombFlag = false;
                    }
                }
                // ���� Ŭ���� ���� ��ź �߻�
                else FireBomb(0);

                _isSingleClick = false;
            }
        }

        if (_isSingleClick && ((Time.time - _timer) > _doubleClickTime))
        {
            FireBomb(0);
            _isSingleClick = false;
        }
    }

    private void OnFireLaser()
    {
        if (Input.GetMouseButton(1) && !_isLongClickLaunch && _fireLaserFlag && !_laser.IsFireLaser)
        {
            _totalChargingTimer += Time.deltaTime;
            Debug.Log("��¡��");

            if (_totalChargingTimer >= _longClickTime)
                _isLongClickLaunch = true; // ��¡ �Ϸ�
        }

        if (Input.GetMouseButtonUp(1) && _isLongClickLaunch)
        {
            FireLaser();
        }
    }

    #endregion

    #region Custom Functions

    private void Move()
    {
        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        Vector3 rotAngle = new Vector3(0, _h, 0);
        transform.Rotate(rotAngle * _rotSpeed * Time.deltaTime);
    }

    private void FireBomb(int weaponIdx)
    {
        var weapon = Instantiate(_weaponPrefs[weaponIdx], _firePos.position, _firePos.rotation);
        Rigidbody _weaponRb = weapon.GetComponent<Rigidbody>();

        switch (weaponIdx)
        {
            case 0:
                _weaponRb.AddForce(weapon.transform.forward * 100f, ForceMode.Impulse);
                Destroy(weapon, 3f);
                break;
            case 1:
                _weaponRb.AddForce(weapon.transform.forward * 50f, ForceMode.Impulse);
                break;
        }
    }

    private void FireLaser()
    {
        _laser.Shot(_firePos);

        _totalChargingTimer = 0;
        _laserCoolTimer = _laserCoolTime;
        UIManager.Instance.UpdateLaserGageDisplay(_laserCoolTimer);

        _fireLaserFlag = false;
        _isLongClickLaunch = false;
    }

    private void Die()
    {
        Instantiate(_expEffect, transform.position, Quaternion.identity);
        _isDead = true;
        gameObject.SetActive(false);
        GameManager.Instance.OnGameFailed("�÷��̾� ���");
    }

    private void GetDamaged(int damage)
    {
        _hp -= damage;
        UIManager.Instance.UpdatePlayerHpDisplay(_hp);

        if (_hp <= 0) 
            Die();
    }

    private void CheckFireBigBombConditions()
    {
        if (_bombCoolTimer <= 0 && !_fireBombFlag)
        {
            _fireBombFlag = true;
            Debug.Log("��ź ��� ����");
        }
        else
        {
            _bombCoolTimer -= Time.deltaTime;
            UIManager.Instance.UpdateBombGageDisplay(_bombCoolTimer);
        }
    }

    private void CheckFireFireLaserConditions()
    {
        if (_laserCoolTimer <= 0)
        {
            _fireLaserFlag = true;
        }
        else
        {
            _laserCoolTimer -= Time.deltaTime;
            UIManager.Instance.UpdateLaserGageDisplay(_laserCoolTimer);

        }

    }

    #endregion

    #region Collider

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        _hitDelay -= Time.deltaTime;

        if (_hitDelay <= 0)
        {
            GetDamaged(1);
            _hitDelay = 0.5f;
        }
    }

    #endregion
}
