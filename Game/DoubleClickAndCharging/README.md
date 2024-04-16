# SJH0421 Game Portfolio
## Double Click And Charging

|타이틀|개발환경|제작기간|플랫폼|키워드|비고|
|:-:|:-:|:-:|:-:|:-:|:-:|
|비행기 맞추기|Unity3D|1day|Windows|Top View, Double Click, Charging||  

![DoubleClick](https://github.com/jhhrnavy/Portfolio/assets/59547352/4f4ce2ed-fd48-4b1b-bb2c-717dc7200907)

### 조작키 
#### 이동  
  A D : 좌우 회전 
#### 공격  
  마우스 좌버튼 클릭 : 작은 폭탄 발사  
  마우스 좌버튼 더블클릭 : 대형 폭탄 발사  
  마우스 우버튼 Hold down(꾹누르기) 2초 : 레이저 발사  
### 승리 조건
제한시간 내에 모든 적 우주선 파괴

### 주요 기능
#### 마우스 우클릭 차징 레이저 발사(Time.time, Particle System)
```swift
    private void OnFireLaser()
    {
        if (Input.GetMouseButtonDown(1) && !_isLongClickLaunch && _fireLaserFlag && !_laser.IsFireLaser)
        {
            _totalChargingTimer = 0f;

            // 차징 VFX
            _chargingEffect.Play();

            // VFX 색상 변경
            var pfxMain = _chargingEffect.main;
            pfxMain.startColor = _chargingColor;
        }

        if (Input.GetMouseButton(1) && !_isLongClickLaunch && _fireLaserFlag && !_laser.IsFireLaser)
        {
            _totalChargingTimer += Time.deltaTime;
            Debug.Log("차징중");

            if (_totalChargingTimer >= _longClickTime && !_isLongClickLaunch)
            {
                _isLongClickLaunch = true; // 차징 완료

                // VFX 색상 변경
                var pfxMain = _chargingEffect.main;
                pfxMain.startColor = _chargeCompleteColor;
            }
        }

        if (Input.GetMouseButtonUp(1) && _fireLaserFlag)
        {
            // 정상적으로 차징이 완료되면 발사
            if (_isLongClickLaunch)
                FireLaser();

            // VFX 끄기
            _chargingEffect.Stop();
        }
    }
```
```swift
    private void FireLaser()
    {
        _laser.Shot(_firePos);

        _totalChargingTimer = 0;
        _laserCoolTimer = _laserCoolTime;
        UIManager.Instance.UpdateLaserGageDisplay(_laserCoolTimer);

        _fireLaserFlag = false;
        _isLongClickLaunch = false;
    }
```
```swift
using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Fields

    [SerializeField] private int damage = 3;
    [SerializeField] private float delay = 0.5f;
    private LineRenderer _laserLineRenderer;
    private Collider _coll;
    private bool _isFireLaser;
    private float _hitDelayTimer = 0f;

    #endregion

    #region Properties
    public bool IsFireLaser { get => _isFireLaser; }

    #endregion

    private void Awake()
    {
        _laserLineRenderer = GetComponent<LineRenderer>();
        _laserLineRenderer.positionCount = 2;
        _laserLineRenderer.enabled = false;
        _coll = GetComponent<Collider>();
        _coll.enabled = false;
        _isFireLaser = false;
    }

    public void Shot(Transform firePos)
    {
        StartCoroutine(ShotEffect(firePos));
    }

    private IEnumerator ShotEffect(Transform firePos)
    {
        _laserLineRenderer.SetPosition(0, firePos.position);
        _laserLineRenderer.SetPosition(1, firePos.position + firePos.forward * 100f);
        _laserLineRenderer.enabled = true;
        _coll.enabled = true;
        _isFireLaser = true;
        yield return new WaitForSeconds(2f);
        _laserLineRenderer.enabled = false;
        _coll.enabled = false;
        _isFireLaser = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _hitDelayTimer -= Time.deltaTime;

            if (_hitDelayTimer <= 0)
            {
                other.GetComponent<Enemy>().GetDamaged(damage);
                _hitDelayTimer = delay;
            }
        }
    }
}
```

#### 마우스 좌 더블클릭 범위폭탄 발사
```swift
    private void OnFireBomb()
    {
        // IsPointerOverGameObject : UI위에서 입력이 들어왔을땐 발사가 되지 않도록 한다
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!_isSingleClick)
            {
                _timer = Time.time;
                _isSingleClick = true;
            }
            else
            {
                // 마우스 좌버튼 더블클릭시 대형 폭탄 발사
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
                // 단일 클릭시 소형 폭탄 발사
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

```
```swift
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
```
