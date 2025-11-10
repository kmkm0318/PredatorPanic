# PredatorPanic

## 프로젝트 개요

---

## 주요 기능
- 상태 기계 기반 캐릭터 컨트롤
- 점프 버퍼, 코요테 타임 등 고급 점프 시스템
- 키보드/컨트롤러 입력 지원

---

## 클래스 구조

### 플레이어
- **Player**: 플레이어 컨트롤러와 플레이어 비주얼 등 플레이어에 필요한 요소들 통합 관리
- **PlayerData (ScriptableObject)**: 플레이어 생성을 위한 플레이어 프리팹, 플레이어 비주얼 프리팹, 플레이어 컨트롤러 데이터 관리
- **PlayerController**: 플레이어 입력 및 상태 관리
- **PlayerControllerData (ScriptableObject)**: 플레이어 컨트롤에 필요한 데이터 관리
- **PlayerBaseState**: 플레이어 상태의 기본 추상 클래스
- **PlayerStateFactory**: 상태 객체 생성 및 관리
- **PlayerVisual**: 플레이어의 비주얼 객체에 붙은 클래스. CameraPivot과 WeaponPivot을 전달하는 역할.
- **PlayerAttack**: Weapon을 저장하고 현재 Weapon을 통해 공격을 수행하는 역할

### 무기
- **Weapon**: 플레이어의 공격에 사용되는 무기를 위한 추상 클래스
- **WeaponData (ScriptableObject)**: 무기 데이터 클래스를 위한 추상 클래스
- **Gun**: 원거리 공격을 위한 클래스. Weapon을 상속함. 히트스캔으로 공격을 하며 Trail을 통해 잔상을 남김. Trail은 ObjectPool을 통해 최적화.
- **GunData (ScriptableObject)**: Gun을 위한 데이터 클래스. WeaponData를 상속함. 공격 속도, 공격력, 사거리, Trail 프리팹, Trail 이동 속도 등을 포함함.

### 상태 기계
- **IState**: 상태 인터페이스
- **StateMachine**: 상태 관리 및 전환

### 매니저
- **GameManager**: 게임 씬의 플레이어 및 카메라 초기화, 싱글톤 관리
- **InputManager**: 입력 시스템 통합 관리