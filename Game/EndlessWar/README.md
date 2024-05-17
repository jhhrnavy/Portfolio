# SJH0421 Game Portfolio
## EndlessWar
>학창시절 자주 했었던 플레시 게임 [끝없는 전쟁](https://vidkidz.tistory.com/2017)을 참조해서 게임의 로직을 구현해 봤습니다.

---------

[플레이 해보기](Game/EndlessWar/Build/EndlessWar.zip)



![비교 플레이 GIF](https://github.com/jhhrnavy/Portfolio/assets/59547352/be4b0e01-c82d-4669-beae-e61503005bea)

----------

### 조작키 
  WASD : 상하좌우 이동
  마우스 움직임 : 플레이어의 조준 방향 변경
  Num1,2,3,4 : 차례대로 주무기, 보조무기, 근접무기, 투척무기
  마우스 좌버튼 클릭 : 공격(발사)
  G : 무기 버리기 ( 주무기, 보조무기 가능 )

------------
  
### 승리 조건
최대한 빠르게 목적지(THIS WAY) 도달

-------------

### 주요 기능 및 활용 기술
#### [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html)
![124124124](https://github.com/jhhrnavy/Portfolio/assets/59547352/830c8882-50a5-4d63-983d-b2e0e6ef51fc)
>Unity의 기존의 Input Manager가 아닌 새로운 Input 관리 기능인 Input System을 활용해서 게임의 Input을 관리해주었다.
  
느낀점  
>간단한 게임을 제작할때엔 기존의 Input Manager가 익숙함과 편의성에서는 약간 우세하다고 느꼈지만  
>다양한 입력장치를 활용하거나 멀티 플랫폼 게임에서의 입력처리면에선 훨씬 나은 편의성을 제공할것 같다.

#### 플레이어    
> PlayerController.cs 로 플레이어의 움직임과 공격 체력을 관리 ( 리펙토링 예정 )
#### 무기 시스템  
> 추상클래스(NewWeapon.cs)로 무기의 속성(무기 이름, 타입, 장착단축키, 위치)을 정의하고 NewGun,NewKnife 등을 통해 무기를 분류  
  
[NewWeapon.cs](Game/EndlessWar/Code/NewWeaponSystem/NewWeapon.cs) : 무기의 속성 정의 ( 무기 이름, 타입, 무기스타일, 오브젝트 위치 )  
[NewGun.cs](Game/EndlessWar/Code/NewWeaponSystem/NewGun.cs) : 충기  
[NewSword.cs](Game/EndlessWar/Code/NewWeaponSystem/NewSword.cs) : 근접 무기  
[Bomb.cs](Game/EndlessWar/Code/Weapons/Bomb.cs) : 투척 무기 (수류탄)  

#### 전투 시스템
>원거리 공격(발사),근거리 공격, 던지기를 추상클래스인 PlayerCombat을 상속 받아 각각 구현  
>다형성을 활용하여 장착한 무기에 따라 다른 공격이 호출  
[BoomShooter.cs](Game/EndlessWar/Code/Weapons/BombShooter.cs) : 투척 공격  
[PlayerMeleeAttack.cs](Game/EndlessWar/Code/Units/Player/Combat/PlayerMeleeAttack.cs) : 근접 공격   
[PlayerShooting.cs](Game/EndlessWar/Code/Units/Player/Combat/PlayerShooting.cs) : 원거리 공격  
