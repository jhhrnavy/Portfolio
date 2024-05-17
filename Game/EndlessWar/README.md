# SJH0421 Game Portfolio
## EndlessWar
>학창시절 자주 했었던 플레시 게임 [끝없는 전쟁](https://vidkidz.tistory.com/2017)을 참조해서 게임의 로직을 구현해 봤습니다.

---------

[플레이 해보기](Game/EndlessWar/Build/EndlessWar.zip)



![DoubleClick](https://github.com/jhhrnavy/Portfolio/assets/59547352/4f4ce2ed-fd48-4b1b-bb2c-717dc7200907)

### 조작키 
  WASD : 상하좌우 이동
  마우스 움직임 : 플레이어의 조준 방향 변경
  Num1,2,3,4 : 차례대로 주무기, 보조무기, 근접무기, 투척무기
  마우스 좌버튼 클릭 : 공격(발사)
  G : 무기 버리기 ( 주무기, 보조무기 가능 )
### 승리 조건
최대한 빠르게 목적지(THIS WAY) 도달

### 주요 기능 및 활용 기술
#### [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html)
![124124124](https://github.com/jhhrnavy/Portfolio/assets/59547352/830c8882-50a5-4d63-983d-b2e0e6ef51fc)
>Unity의 기존의 Input Manager가 아닌 새로운 Input 관리 기능인 Input System을 활용해서 게임의 Input을 관리해주었다.
> 느낀점
>간단한 게임을 제작할때엔 기존의 Input Manager가 익숙함과 편의성에서는 약간 우세하다고 느꼈지만  
>다양한 입력장치를 활용하거나 멀티 플랫폼 게임에서의 입력처리면에선 훨씬 나은 편의성을 제공할것 같다.  
#### 무기 시스템
```swift
using UnityEngine;

public abstract class NewWeapon : MonoBehaviour
{
    public string name;
    public WeaponType weaponType;
    public WeaponStyle weaponStyle;

    public Vector3 localPosition; // 무기 장착 위치
    public Vector3 localRotation;
    public Vector3 localScale;

    [Header("Anim IK"), Space] // Animation IK 적용 Transform
    public Transform trsfRHandMount;
    public Transform trsfLHandMount;
}

public enum WeaponType // 무기 종류
{
    Melee,
    Pistol,
    AR,
    Bomb
}

public enum WeaponStyle
{
    None = -1,
    Primary,    // 주무기
    Secondary,  // 보조무기
    Melee,      // 근접무기
    Throwing    // 투척무기
}

```
