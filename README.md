# PredatorPanic

## 프로젝트 개요
Unity 3D로 개발된 로그라이트, 뱀파이어 서바이벌 라이크 게임입니다.
라운드마다 강해지는 적, 보스를 쓰러뜨리고 경험치와 재화를 획득해서 보상을 선택하거나 아이템을 구입하여 강해질 수 있습니다.
한 번의 게임에서만 사용할 수 있는 재화 말고도 영구적으로 얻을 수 있는 재화를 통해서 플레이어를 더욱 강화할 수 있습니다.
시작 시에도 플레이어 캐릭터, 시작 무기 등을 구입해서 사용할 수 있습니다.
다량의 캐릭터, 탄막, 효과 등을 처리하는 것과 Scriptable Object를 활용한 데이터 중심 구조에 초점을 두었습니다.
추가적으로 게임 로직 및 플레이어 로직에 FSM이, UI에는 MVP와 같은 패턴도 적용되어 있습니다.

---

## 주요 기능
### FSM
- 플레이어의 움직임을 제어하는 데에 Character Controller와 FSM이 사용되었습니다. Grounded, Jump, Fall 등과 같은 기본적인 상태와 그 하위에 있는 Idle, Move와 같은 세부 상태를 활용하였습니다.
- 게임 진행 로직에도 FSM이 사용되었습니다. RoundStart, Playing, Pause, Shop 등의 상태를 따로 분리하여 깔끔하게 정리하였습니다.

### MVP
- 게임 내에서 플레이어 상태 UI, 정지 UI, 상점 UI, 레벨 업 보상 선택 UI 등을 MVP 패턴을 적용해 구현하였습니다.
- View는 UI 표시 및 UI 이벤트에 집중
- Model은 데이터 관리 및 데이터 이벤트에 집중
- Presenter가 그 사이를 연결해주는 역할을 하였습니다.

### Scriptable Object를 통한 데이터 위주의 설계
- Player, Weapon, Bullet, Enemy, DropItem, Audio 등 Scriptable Object를 적용할 수 있는 여러 부분에 Scriptable Object를 최대한 활용하였습니다.
- 오브젝트 풀링을 구현할 때 Dictionary<Data, ObjectPool<Prefab>>을 통해서 다양한 데이터에 대해서 오브젝트 풀을 따로 구현할 수 있게 하였습니다.

### Stat, Stat Modifier 시스템
- 기본적인 Stat과 Modifier를 통해서 Stat을 강화할 수 있도록 했습니다. 이때 Modifier Type에 FlatAdd, PercentAdd, PercentMult를 통해서 다양한 강화를 할 수 있도록 하였습니다.
- Stats<T> 클래스는 Enum T에 대해서 Dictionary<T, Stat>를 통해서 제너럴하게 여러 타입에 대해서 Stat을 적용할 수 있도록 하였습니다.
- Player, Weapon, Enemy에는 이러한 스탯이 적용되어 있으며 Player는 레벨 업 보상 및 아이템을 통해서, Enemy는 라운드의 진행에 따라서 Stat이 강화되도록 설정하였습니다.

---

## 최적화 기법
### ManualUpdate
Bullet, DropItem 등에서 Update를 제거하고 IManualUpdate를 통해서 각 Manager 클래스가 Update에서 활성화 상태인 객체들을 순회합니다. 이때 비활성화 대상 오브젝트를 리스트의 그 위치에서 바로 없애는 것이 아니라 리스트의 맨 뒤의 오브젝트를 해당 위치의 인덱스로 가져오고 맨 뒤의 오브젝트를 제거하는 것으로 최적화를 진행했습니다.

### PhysicalUtility 클래스
자주 사용하는 OverlapSphere, Raycast를 위해서 버퍼를 클래스, 객체마다 따로 두고 사용하는 것이 아니라 중앙에서 하나의 버퍼를 두고 공유해서 사용합니다. 이때 NonAlloc을 사용하는 것으로 GC 부담을 최소화 하였습니다.

### 렌더링 최적화
- Skinned Mesh Renderer를 사용할 경우 표현해야 할 애니메이션이 많아지기 때문에 LOD Group을 통해서 메인 카메라에 작게 보이는 캐릭터는 일반 Mesh Renderer를 사용하도록 했습니다.
- 같은 메시와 같은 Material일 때 Material의 GPU Instancing을 사용하는 것으로 배치 수를 최대한 줄였습니다.
- 배칭을 깨지 않으면서 Material을 변경해주기 위해 Material Property Block을 사용하였습니다.


### 아이콘 이미지 출처
https://game-icons.net
- Lorc, http://lorcblog.blogspot.com
- Delapouite, https://delapouite.com
- John Colburn, http://ninmunanmu.com
- Felbrigg, http://blackdogofdoom.blogspot.co.uk
- John Redman, http://www.uniquedicetowers.com
- Carl Olsen, https://twitter.com/unstoppableCarl
- Sbed, http://opengameart.org/content/95-game-icons
- PriorBlue
- Willdabeast, http://wjbstories.blogspot.com
- Viscious Speed, http://viscious-speed.deviantart.com - CC0
- Lord Berandas, http://berandas.deviantart.com
- Irongamer, http://ecesisllc.wix.com/home
- HeavenlyDog, http://www.gnomosygoblins.blogspot.com
- Lucas
- Faithtoken, http://fungustoken.deviantart.com
- Skoll
- Andy Meneely, http://www.se.rit.edu/~andy/
- Cathelineau
- Kier Heyl
- Aussiesim
- Sparker, http://citizenparker.com
- Zeromancer - CC0
- Rihlsul
- Quoting
- Guard13007, https://guard13007.com
- DarkZaitzev, http://darkzaitzev.deviantart.com
- SpencerDub
- GeneralAce135
- Zajkonur
- Catsu
- Starseeker
- Pepijn Poolman
- Pierre Leducq
- Caro Asercion

### 동물 모델 출처
"Elephant (Low_Poly)" (https://skfb.ly/pwqWV) by planeta-elefante is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Low-poly animated rabbit" (https://skfb.ly/oHI9x) by Pneshik is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).