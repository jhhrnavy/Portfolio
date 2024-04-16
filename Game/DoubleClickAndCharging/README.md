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
