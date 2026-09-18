### ⚠️ IMPORTANT NOTICE / DISCLAIMER

**Original Author:** TacticalToaster
**Original Repository:** BlackDiv
**Original Link:** https://github.com/TacticalToaster/BlackDiv
**License:** MIT
**This Port By:** R_F (danyhappy564-cmyk) — unofficial, AI-assisted port. Not affiliated with or endorsed by the original author.

1. **Reflection & Take-Downs:** I deeply reflect on the ECOT incident. As an AI-assisted "vibe coder," I will immediately delete files if the original authors ask.
2. **No Re-Distribution:** These ported builds are unverified, temporary fixes. Please do NOT re-upload or share them anywhere else.
3. **Do Not Pester Original Authors:** Never report bugs or pester original modders regarding issues from my unofficial ports.
4. **Full Credit & Respect:** I will always credit original creators on GitHub and prioritize their decisions above all else.
5. **Support Original Creators:** Instead of using my ports, please visit the original authors' Forge pages to leave kind words or tips.

---

# BlackDiv (fork)

> **원작자 · 원본 레포**
> **TacticalToaster** — https://github.com/TacticalToaster/BlackDiv
>
> **라이선스: MIT** (Copyright (c) 2025 Matthew Ryan Christensen II)
>
> 이 레포는 위 원작의 **포크**입니다. 봇도 로드아웃도 퀘스트도 전부 원작자의 것이고,
> 여기서 한 건 SAIN이 블랙디비전에 안 붙던 문제를 고치고 빌드 경로를 이식 가능하게
> 만든 것뿐입니다.

MoreBotsAPI 기반으로 Black Division 팩션을 추가하는 모드입니다.

현재 기준: **upstream 1.3.1 / SPT 4.1**

---

## 이 포크가 원작과 다른 점

| 수정 | 내용 |
|---|---|
| `SainBrainLayerPatch` | 블디/웨지에 SAIN이 전혀 안 붙던 문제. 원작 1.3.x가 넣은 서버측 등록만으로는 안 고쳐집니다 (아래 26/09/07 항목) |
| `SptRoot` MSBuild 속성 | 세 csproj의 `..\..\..\` 상대경로 제거 |
| 서버 배포 경로 | `$(SptRoot)\SPT_Runtime\user\mods\` — 4.1에서 서버가 `SPT_Runtime\` 밑으로 옮겨간 것 반영 |
| `BDSteeringHandoffDiagnostic` | 블디/웨지 봇이 SAIN 통제를 벗어나는 순간을 봇당 1회 로그로 남김 (아래 26/09/18 항목) |
| `BDUnderFireSteeringFallback` | 도주 중 SAIN을 놓친 블디/웨지 봇이 피격당해도 안 돌아보던 문제 수정 (아래 26/09/18 항목) |

---

## 빌드

```
dotnet build BlackDiv.sln
```

경로는 각 csproj의 `SptRoot`에서 나옵니다. 기본값 `E:\SPT 4.1`, `-p:SptRoot=...`
또는 동명의 환경변수로 덮어쓸 수 있습니다.

| 프로젝트 | 배포 위치 |
|---|---|
| `Plugin` | `$(SptRoot)\BepInEx\plugins\BlackDiv\` |
| `Prepatch` | `$(SptRoot)\BepInEx\patchers\` |
| `Server` | `$(SptRoot)\SPT_Runtime\user\mods\BlackDivServer\` |

`Plugin`은 설치된 `MoreBotsPlugin`/`SAIN`/`DrakiaXYZ-BigBrain`을, `Server`는 설치된
`MoreBotsServer`를 참조합니다. 레포의 `Reference/MoreBotsServer/MoreBotsServer.dll`은
4.0 시절 것이라 최신 `MoreBotsServer.Interop` 타입이 없습니다 — 원작이 4.1에서 참조를
설치본 쪽으로 옮긴 이유입니다. 빌드에는 안 쓰이니 그대로 둡니다.

원작 1.3.x부터 `Server` 프로젝트도 솔루션에 들어 있어서, `BlackDiv.sln` 하나로 세
프로젝트가 같이 빌드됩니다.

### `Ambiguous project name 'BlackDiv'`

`obj/`가 비어 있는 상태에서 복원하면 나던 오류입니다. `Plugin`과 `Prepatch`가 **의도적으로**
같은 `AssemblyName`(`BlackDiv`)을 쓰는데, SDK 스타일 프로젝트에서 `PackageId`가
`$(AssemblyName)`을 기본값으로 따라가는 바람에 두 프로젝트가 복원 그래프에서 같은 이름을
주장하게 됩니다. 그러면 `project.assets.json`이 아예 안 만들어져서 `Plugin` 빌드도 같이
죽습니다 (`NETSDK1004`).

`PackageId`를 각각 `BlackDiv.Plugin` / `BlackDiv.Prepatch`로 명시해서 해결했습니다.
**`AssemblyName`은 둘 다 `BlackDiv` 그대로**라 출력 파일명은 안 바뀝니다 — 여기서
`AssemblyName`을 갈라놓으면 `Plugin`의 출력 폴더에 DLL이 하나 더 떨어져서 실제 배포
구성이 달라집니다. 패키징은 안 하니 `PackageId`는 그 외에는 아무 영향이 없습니다.

재현/확인:

```
rm -rf */obj && dotnet restore BlackDiv.sln
```

---

## 변경점

<26/09/04 상세 변경점 — 포크 수정사항>

- **블랙디비전/웨지에 SAIN이 전혀 적용되지 않던 문제 수정** (`SainBrainLayerPatch`)

  증상: SAIN 설치·설정이 정상인데도 블디와 웨지만 계속 바닐라 AI로 싸움. BigBrain
  디버그 오버레이로 보면 진짜 PMC는 `Layer:SAIN : Combat Layer`인데, 블디는
  `Layer:Pmc`, `AdvAssaultTarget`, `AssaultHaveEnemy` 같은 바닐라 레이어만 뜸.
  맵과 무관하게(쇄빙선/랩 동일) 재현.

  원인은 **등록 문제가 아니라 우선순위 문제**였음. BigBrain 레지스트리를 덤프해보니
  SAIN 레이어는 블디 6종 역할 전부에 이미 정상 등록되어 있었고(MoreBotsAPI의
  `AddSAINLayers()`는 제 역할을 하고 있었음), 각 봇에 SAIN `BotComponent`도 붙어
  있었음. 문제는 이것:

  ```
  SAIN CombatSoloLayer    prio 20
  SAIN CombatSquadLayer   prio 22
  바닐라 Pmc / AdvAssaultTarget / AssaultHaveEnemy    ← 훨씬 위
  ```

  SAIN은 바닐라를 **이기도록** 만들어진 게 아니라 **제거해서 자리를 비우는** 구조라,
  그 제거가 없으면 20/22짜리 레이어는 평생 차례가 안 옴. MoreBotsAPI도 그 제거를
  요청하지만 `TarkovApplication.Init` 시점 1회뿐이라, 그 뒤에 도는 SAIN 자신의
  `BigBrainHandler.Init()`이 제외 목록을 다시 만들면서 덮어써 버림.

  수정: 라이드 시작(`GameWorld.OnGameStarted`) 시점에 SAIN 레이어 등록 + 바닐라
  전투 레이어 제외를 다시 적용. 부수효과를 없애려고 SAIN 래퍼 대신
  `BrainManager.RemoveLayers`를 직접 호출.

  적용 범위는 **브레인 `PMC` + 블디 6종 역할로만** 한정. 개발 중 브레인 목록에
  `ExUsec`를 같이 넣었다가 **진짜 로그(Rogue)들이** `PatrolFollower`만 남아 서로 졸졸
  따라다니며 한곳에 뭉쳐 멈추는 사고가 있었음 (덤으로 `PersonActiveClass.CheckAlive`
  NRE가 라이드당 4000회 폭주).

  > **26/09/07 정정**: 당시 원인을 "`ExUsec` 브레인이 통째로 벗겨져서"로 적었는데,
  > 그 커밋(`3887051`)을 다시 읽어보니 **역할 인자를 똑같이 넘기고 있었습니다**. 역할
  > 스코프가 걸려 있었으면 진짜 로그(역할 `exUsec`)는 애초에 안 걸렸어야 합니다.
  > 그 시도는 브레인 목록 말고도 SAIN의 `ToggleVanillaLayersForBrainsAndRoles` 래퍼를
  > 거쳤다는 차이(`RestoreLayers`를 추가로 호출)가 있었고, 둘 중 뭐가 로그를 망가뜨린
  > 건지는 끝내 못 갈랐습니다. 확실한 건 **지금 형태(브레인 `PMC` 단독 +
  > `BrainManager.RemoveLayers` 직접 호출)가 실전 라이드에서 로그 정상 동작과 함께
  > 검증됐다**는 것뿐이라, 넓히지 않고 그대로 둡니다.

  검증(실전 라이드): `blackDivIb`/`bossWedge` 모두 `SAIN : Combat Layer`,
  `SAIN : Avoid Threat` 진입 확인. 쇄빙선 자체 레이어(`IceCrewRush`/`IceCrewHold`/
  `WedgeRooms`)도 그대로 번갈아 작동. `ExUsec` 브레인 제외 0건, SAIN NRE 0건.

- **빌드 경로 하드코딩 제거** (`Plugin`/`Prepatch`/`Server` csproj)

  `..\..\..\` 상대경로로 박혀 있어서 폴더 깊이가 다르면 게임 어셈블리를 못 찾던 문제.
  `SptRoot` MSBuild 속성으로 빼서 `-p:SptRoot=...` 또는 환경변수로 덮어쓸 수 있게 함
  (기본값 `E:\SPT 4.0.10`). `DrakiaXYZ-BigBrain`/`SAIN` 참조가 `..\..\plugins\...`로
  `BepInEx` 경로 한 단계를 빠뜨리고 있던 것도 같이 정정.

  참고: `Prepatch`의 `AssemblyName`은 `Plugin`과 동일하게 `BlackDiv`로 두어야 함.
  NuGet의 `Ambiguous project name` 오류를 피하려고 잠깐 다른 이름으로 바꿨더니,
  `Plugin`이 `Prepatch`를 `ProjectReference`(기본 `Private=true`)로 참조하는 탓에
  빌드 출력에 DLL이 하나 더 생겨서 실제 배포 파일 구성이 달라짐. 그 오류는 여기서
  말고 복원 쪽에서 해결할 것.

---

<26/09/07 상세 변경점>

- 원작 1.3.1 (SPT 4.1)을 **머지**로 받음. 충돌은 csproj 3개뿐이었고 `SainBrainLayerPatch`
  와 `Plugin.cs` 등록은 자동 머지됨. 충돌 처리:
  `Prepatch`의 `TargetFramework`는 원작 것(`net471` → `netstandard2.1`) 채택,
  `HintPath`와 PostBuild는 이쪽의 `SptRoot` 형태 유지(체크아웃 위치에 안 묶이는 쪽),
  `MoreBotsPlugin` 참조는 원작을 따라 레포 내 `Reference/` 대신 설치본을 보게 하되
  경로만 `$(SptRoot)`로 통일. `SptRoot` 기본값 `E:\SPT 4.0.10` → `E:\SPT 4.1`

- **서버 배포 경로가 4.0 레이아웃에 멈춰 있던 것 수정.** 이쪽 PostBuild가
  `$(SptRoot)\user\mods\`를 쓰고 있었는데, 4.1에서 서버가 `SPT_Runtime\` 밑으로
  옮겨갔습니다 (원작의 상대경로 `..\..\..\SPT_Runtime\user\mods\`가 근거).
  `$(SptRoot)\SPT_Runtime\user\mods\`로 정정

- **원작 1.3.x의 SAIN 작업이 이 패치를 대체하지 못하는 이유 확인.** 원작이
  `Server/SAIN/BlackDivSainRegistrations.cs`를 새로 넣어서 블디 6종 역할을
  MoreBotsAPI의 `SainInteropRegistration`에 등록합니다. 방향은 맞고 이 포크도 그대로
  두지만, 증상은 안 고쳐집니다:

  - **레이어 목록은 원래부터 문제가 아니었음.** MoreBotsAPI가 등록값 앞에 자기
    `commonVanillaLayersToRemove`(`Help`, `AdvAssaultTarget`, `Hit`, `Simple Target`,
    `Pmc`, `AssaultHaveEnemy`, `Assault Building`, `Enemy Building`, `PushAndSup`,
    `Pursuit`)를 붙이므로, 실제로 봇을 잡고 있던 `Pmc`/`AdvAssaultTarget`/
    `AssaultHaveEnemy`는 이미 요청에 들어 있습니다. 이 패치의
    `VanillaLayersToExclude` 16개는 그 합집합과 **정확히 일치**하도록 맞춰뒀습니다
    (스크립트로 대조 확인).
  - **문제는 타이밍.** MoreBotsAPI는 이 전부를 `TarkovApplication.Init` 포스트픽스
    한 번에 적용하는데, 그 뒤에 도는 SAIN 자신의 `BigBrainHandler` 초기화가 제외
    목록을 다시 만들면서 덮어써 버립니다. 라이드 시작(`GameWorld.OnGameStarted`)에
    다시 적용하는 게 이 패치의 존재 이유입니다.
  - MoreBotsAPI 2.1.1의 interop 자체가 반쯤 꺼져 있습니다 — 4.1 커밋 제목이
    "4.1 update (minus SAIN interop being broken AF)"이고, `CreateCustomBotTypes`
    안의 `BotTypeDefinitions.AddBotType`과 `AddBotTypeToSettings`가 둘 다 주석
    처리되어 있습니다

- **4.1 API 확인.** 이 패치가 부르는 두 함수 모두 살아 있습니다. SAIN 4.5.1의
  `BigBrainHandler.ToggleVanillaLayers`가 `BrainManager.RemoveLayers(layerNames,
  brainNames, roles)` — 이 패치가 쓰는 3인자 역할 스코프 오버로드 — 를 그대로
  호출하고, MoreBotsAPI 2.1.1이 `AddCustomLayersToBrainsAndRoles`를 같은 시그니처로
  부릅니다. 역할 ID 6개도 원작 등록값과 일치 확인

- 09/04 항목의 `ExUsec` 원인 설명을 정정 (해당 항목 안 인용 블록 참고)

- 검증: `Server` 프로젝트를 실제 SPT 4.1 패키지 + 최신 `MoreBotsServer`로 빌드해
  에러 0 확인. `Plugin`/`Prepatch`는 EFT 어셈블리가 필요해서 여기서는 구문 파싱
  (16개 파일, 에러 0)과 심볼 대조까지만 — 실기 빌드 필요

- **`Ambiguous project name 'BlackDiv'` 복원 오류 수정.** `obj/`를 지우고 복원하면
  재현됩니다(이 컨테이너에서 재현 → 수정 → 3개 프로젝트 전부 복원 성공까지 확인).
  `Plugin`과 `Prepatch`가 같은 `AssemblyName`을 쓰는데 `PackageId`가 그걸 기본값으로
  따라가서 생기는 충돌이라, `PackageId`만 각각 `BlackDiv.Plugin`/`BlackDiv.Prepatch`로
  명시했습니다. `AssemblyName`은 둘 다 `BlackDiv` 유지 — 출력 파일명과 배포 구성은
  그대로입니다. 09/04에 "복원 쪽에서 해결할 것"이라고만 적어두고 미뤄뒀던 것을
  실제로 해결한 것입니다

---

<26/09/18 상세 변경점>

- **블디/웨지 봇이 도주 중 엎드린 채로 피격에도 안 돌아보던 문제 수정**
  (`BDSteeringHandoffDiagnostic`, `BDUnderFireSteeringFallback`)

  증상: 블디/웨지 봇이 총격전 중 은폐 지점으로 도주 → 엎드림까지는 정상인데,
  이후 총을 맞아도 그 방향으로 고개/몸을 안 돌림. 죽지도 않고 그냥 그 자리에
  얼어붙은 것처럼 굳어 있음.

  **원인 사슬** (SAIN 소스 직접 대조로 확인):

  1. 블디/웨지 봇의 장비 구성이 바닐라 `BotReload.CanReload`가 기대하는 슬롯
     배열과 안 맞아서, 재장전 시도할 때마다 예외가 남
     (`SelfActionDecisionClass.TryReload`, SAIN 쪽에 이미 백오프+로그로 격리되어
     있음 — 이건 SAIN 저장소의 기존 이력이고 이 포크에서 고친 건 아님). 결과적으로
     그 봇은 **영원히 재장전에 성공하지 못함**.
  2. 총알이 없으면 SAIN의 `EnemyDecisionClass`가 무조건 `Retreat` 판정 →
     `SeekCoverAction`으로 도주+은폐(엎드림까지 여기서 일어남).
  3. 이후 적을 놓쳐 SAIN의 `GoalEnemy`가 `null`이 되면, SAIN의 전투/위협회피
     레이어가 전부 비활성화됨.
  4. `SainBrainLayerPatch`가 블디/웨지용 **바닐라 폴백 레이어를 통째로 제거**해
     놨기 때문에(SAIN을 돌리려면 필수였던 조치, 26/09/04 항목 참고), SAIN도 꺼지고
     바닐라도 없는 상태에서 남는 건 이 포크 자체의 `HuntTargetLayer`
     (`Plugin.cs`에 우선순위 10으로 등록)뿐.
  5. `MoreBotsAPI_Check`(원본 소스)로 직접 확인: `HuntTargetLayer`가 미는
     `HuntTargetAction.Update`는 `BotOwner.Steering.LookToMovingDirection()` +
     제네릭 `LookAround`뿐이라 "쏜 사람 쪽으로 돌아보기" 로직이 아예 없음. 게다가
     SAIN의 `SAINMoverClass.ManualUpdate`는 `Bot.SAINLayersActive`가 꺼지면 자기
     조향 적용 코드(`TickPlayerSteering()`)를 통째로 스킵함. 결과: 봇의 시선이
     그 순간 방향에 얼어붙고, 총을 맞아도 안 돌아봄.

  이건 최근 SAIN 변경이 아니라 **이 포크가 바닐라 폴백을 없애면서 생긴 구조적
  빈틈**입니다. 다른 SAIN 관리 봇(일반 PMC 등)은 바닐라 폴백이 남아 있어서 이
  증상이 안 나옵니다.

  **수정 1차 시도 (실패, 기록으로 남김):** `PlayerComponent.CharacterController
  .SetTargetLookDirection(...)` — SAIN이 조향을 실제로 적용할 때 쓰는 것과 같은
  호출을 그대로 흉내 냈으나, 이건 `Bot.SAINLayersActive == true`일 때만 의미가
  있음. `SAIN/Patches/Shoot/AimDataPatches.cs`의 `SmoothTurnPatch`를 다시 읽어보니,
  `SAINLayersActive == false`일 때는 오히려 **바닐라의 `_lookDirection` 값을 SAIN의
  `TurnData`로 복사만 하고** 원본 바닐라 메서드를 그대로 실행시킴 — 즉 1차 수정이
  써놓은 값은 매 프레임 이 동기화 로직에 그대로 덮어써져서 캐릭터한테 도달하지
  못함. 컴파일도 되고 실행도 되지만 **눈에 보이는 효과가 전혀 없는 코드**였음.
  실전 라이드 없이 코드만 다시 읽어서 잡음 (`SPT-BigBrain_Check`,
  `MoreBotsAPI_Check`를 참고용으로 클론한 뒤 발견).

  **수정 2차 (현재 적용본):** 실제로 회전을 결정하는 건 바닐라 `BotSteering
  ._lookDirection` 필드 자체. SAIN 본인도 `SmoothTurnPatch`에서 이 필드에 직접
  (`__instance._lookDirection = ...`) 쓰고 있어서, 별도 어셈블리에서 접근 가능한
  게 이미 검증된 사실. `BDUnderFireSteeringFallback`이 블디/웨지 6종 역할에만,
  `HuntTargetAction.Update`의 postfix로 붙어서, 바닐라 `BotOwner.Memory
  .IsUnderFire`가 켜져 있고 SAIN이 그 틱에 조향을 안 하고 있으면(`SAINLayersActive
  == false`) 이 필드를 직접 피격 방향으로 설정함. 사용한 지점/공식은 전부 SAIN
  자체 소스(`SAINSteeringClass.LookToUnderFirePos`/`WeaponRootOffset`)에서 그대로
  가져온 것 — 추측 없음.

  `BDSteeringHandoffDiagnostic`은 진단 전용으로 남겨둠: 봇당 1회
  `SAINLayersActive=false` 전이 시점의 상태(`ActiveLayer`, `GoalEnemy`, 탄약/재장전
  상태, 피격 여부)를 로그로 남기고, `BDUnderFireSteeringFallback`도 실제로
  발동했을 때 봇당 1회 로그를 남김. "증상이 안 보임"과 "수정이 실제로 돎"을
  구분하기 위한 것 (1차 수정이 바로 이 구분 없이 "고친 것처럼 보였다가" 코드
  재검토로 무효였음이 드러난 사례라 더 필요해짐).

  검증: 코드 레벨로는 확정. 실전 라이드에서 `[BDUnderFireSteeringFallback]
  engaged for ...` 로그가 찍히는지는 아직 미확인 — 다음 리포트에서 확정.

  재장전 예외 자체(위 1번)는 이번에 손대지 않음. 그건 블디 로드아웃 장비 슬롯
  데이터 문제라 별도 작업 필요.
