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

## 변경 이력

- 2026-09-24 21:44 (KST) — **"도망가서 엎드린 봇이 총 맞아도 쏜 쪽을 안
  돌아보던 버그"의 수정(09-18)이 사실 제대로 작동하지 않고 있었던 걸 발견해서
  다시 고침.** 게임 엔진의 회전 처리 코드를 직접 뜯어보니, 이전 수정이 넣어준
  "바라볼 방향" 값을 엔진이 매 프레임 이동 방향으로 덮어쓰거나(움직이는 중),
  아예 회전을 건너뛰고(멈춰 있는 중) 있었음. 이제는 엔진한테 "이 지점을
  바라봐라"라고 정식 명령을 내리는 방식으로 바꿔서 실제로 몸을 돌림. 또 이
  버그를 추적하려고 넣어둔 진단 로그가 스폰 직후 평범한 대기 상태를 기록하느라
  정작 버그 순간은 못 남기던 문제도 고침(이제 총 맞는 중일 때만 기록).
- 2026-09-21 15:21 (KST) — 블디/웨지 성격을 기가차드로 강제하는 기능이 실제로
  작동했는지 로그로 확인할 방법이 없었던 문제 고침 — 성공했을 때 아무 로그도
  안 남기고 실패했을 때만 로그를 남기게 짜여 있어서, 라이드 로그를 봐도
  "안 됐다"와 "그냥 블디가 안 나왔다"를 구분할 수 없었음. 이제 성공하면
  봇당 한 번씩 로그를 남김. 동작 자체(기가차드로 바꾸는 로직)는 안 바뀜.
- 2026-09-19 06:47 — 실제로 코드를 컴파일해서 검증함(그동안은 게임 파일을
  직접 뜯어보는 방식으로만 확인했음). 쇄빙선(Icebreaker) 맵을 같이 쓸 때
  블디가 어떻게 동작하는지도 문서로 정리함. 동작 자체가 바뀐 건 아님.
- 2026-09-19 06:01 — 블디(리드/어썰트/브리처/서포트/레이더)와 웨지(보스)의
  SAIN 성격을 F12 설정에서 각각 따로 켜고 끌 수 있게 함. 기본값은 둘 다
  "기가차드"(제일 공격적인 성격) — 팩션 설정상 원래 괴물급이어야 하는데
  기존엔 성격이 거의 랜덤(대부분 평범, 3% 확률로만 기가차드)으로 배정되던
  문제 고침.
- 2026-09-19 05:44 — SAIN도, 블디 자체 추적 대상도 둘 다 없어지는 순간
  봇이 그냥 그 자리에 멈춰 서 있던 마지막 빈틈을 메움 — 이제 이 상황에서도
  최소한 순찰은 돌게 됨.
- 2026-09-19 05:25 — "재장전이 가끔 실패한다"는 문제의 원인을 정정함.
  원래는 블디 장비 슬롯 자체의 문제로 추정하고 문서에 그렇게 적어뒀었는데,
  실제로는 다른 모드(Use Items Anywhere 2.1.4)가 게임 내부 데이터를
  깨뜨려서 생긴 문제였음이 밝혀짐(그 모드 쪽에서 이미 수정됨). 문서만
  정정, 예방 코드는 그대로 유지.
- 2026-09-18 07:33 / 06:55 / 06:49 — **적한테 쫓기다 도망쳐서 엎드린 봇이,
  그 상태에서 총을 맞아도 전혀 반응하지 않고 완전히 멍하니 있던 버그를
  고침.** 1차 시도는 겉보기엔 고쳐진 것 같았지만 실제로는 게임 엔진이
  그 수정을 매 프레임 무시하고 있던 걸 뒤늦게 발견해서, 원인을 다시 찾아
  제대로 된 방식으로 재수정함.
- 2026-09-16 13:43 — SAIN(다른 AI 강화 모드) 없이도 블디가 죽지 않고
  실행되도록 의존성 분리, 버전 올림.
- 2026-09-11 — 배포용 압축파일(zip) 자동 생성 기능 추가, 원작자 표기/고지
  문구 추가.
- 2026-09-06 — SPT 4.1 버전 대응 업데이트.

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

현재 기준: **upstream 1.3.2 / SPT 4.1**

---

## 이 포크가 원작과 다른 점

| 수정 | 내용 |
|---|---|
| `SainBrainLayerPatch` | 블디/웨지에 SAIN이 전혀 안 붙던 문제. 원작 1.3.x가 넣은 서버측 등록만으로는 안 고쳐집니다 (아래 26/09/07 항목) |
| `SptRoot` MSBuild 속성 | 세 csproj의 `..\..\..\` 상대경로 제거 |
| 서버 배포 경로 | `$(SptRoot)\SPT_Runtime\user\mods\` — 4.1에서 서버가 `SPT_Runtime\` 밑으로 옮겨간 것 반영 |
| `BDSteeringHandoffDiagnostic` | 블디/웨지 봇이 SAIN 통제를 벗어나는 순간을 봇당 1회 로그로 남김 (아래 26/09/18 항목) |
| `BDUnderFireSteeringFallback` | 도주 중 SAIN을 놓친 블디/웨지 봇이 피격당해도 안 돌아보던 문제 수정 (아래 26/09/18 항목) |
| `BDIdlePatrolLayer` / `BDIdlePatrolAction` | SAIN도 사냥 대상도 둘 다 없어서 아무 레이어도 안 걸리는 봇에게 바닐라 순찰이라도 강제 (아래 26/09/19 항목) |
| `BDPersonalityOverride` + `BDPersonalityConfig` | 블디(웨지 제외 5종)/웨지 SAIN 성격을 F12에서 각각 따로 설정 가능, 기본값 GigaChad (아래 26/09/19 항목) |

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

  재장전 예외 자체(위 1번)는 이번엔 손대지 않았고, **블디 로드아웃 장비 슬롯
  문제라는 추정은 틀렸음이 밝혀짐**. 실제 원인은 SAIN 쪽에서 별도로 규명됨:
  `Use Items Anywhere` 2.1.4가 BSG의 static 필드 `Inventory.FastAccessSlots`를
  교체가 아니라 병합해버려서, 봇 포함 게임 내 모든 인벤토리가 계속 길어지는
  같은 배열을 공유하게 되는 게 원인이었음(SAIN 레포 README 9번 참고).
  **2026-09-19 기준 해당 모드 쪽에서 수정 완료** — UIA 버전을 올리면 이 예외는
  더 이상 안 남. SAIN의 백오프/로그 자체는 다른 모드가 같은 static을 다시
  건드릴 경우를 대비해 그대로 유지.

---

<26/09/19 상세 변경점>

- **SAIN도 사냥 대상도 둘 다 없을 때 봇이 그냥 멈춰있던 마지막 빈틈 메움**
  (`BDIdlePatrolLayer`, `BDIdlePatrolAction`)

  위 26/09/18 항목에서 다룬 흐름의 마지막 단계: SAIN이 `GoalEnemy`를 놓치고,
  **`HuntTargetLayer`의 `BotHuntManager.HasHuntTarget()`까지 꺼지면**(사냥
  대상이 죽었는데 대체할 살아있는 적 사이드가 더 없는 경우) 이 봇한테 걸리는
  레이어가 하나도 없어집니다. `SainBrainLayerPatch`가 바닐라 폴백 레이어를
  이 역할들 한정으로 이미 제거해뒀기 때문에, BigBrain은 그 틱에 활성 레이어를
  못 찾고(`BotBaseBrainUpdatePatch`의 "no layers are active" 분기) 마지막
  행동을 그대로 유지합니다. 크래시는 아니지만, 아무도 새 지시를 안 내리니
  사실상 멈춘 것과 구분이 안 됩니다.

  수정: `BDIdlePatrolLayer`를 우선순위 5(=`HuntTargetLayer`의 10, SAIN의
  모든 레이어보다 아래)로 등록. 이 레이어는 그 위 레이어들이 전부 안 걸릴
  때만 걸리는 최후의 보루라서 `IsActive()`는 생존 체크만 하고 무조건
  `true`. 실제 행동(`BDIdlePatrolAction`)은 MoreBotsAPI의
  `SearchForTargetAction`이 이미 하는 것과 똑같은 호출
  (`AIActionsList.CreateNode(BotLogicDecision.simplePatrol/followerPatrol,
  botOwner)`)로 바닐라 순찰 노드를 그대로 돌립니다 — 새로 추측한 게 아니라
  이미 검증된 패턴을 재사용.

  여기서 쓴 바닐라 멤버(`AIActionsList.CreateNode`, `AICoreNode
  .UpdateNodeByMain`, `BotLogicDecision`의 두 필드, `BotOwner.Boss.IamBoss`,
  `BotOwner.HealthController`)는 전부 실제 `Assembly-CSharp.dll`을
  `dnfile`로 열어서 접근성까지 직접 확인한 뒤 작성함 — MoreBotsAPI 쪽 사용
  예시만 보고 베낀 게 아님.

  검증: 코드 레벨/멤버 접근성은 확정. 실전 라이드에서 이 상황(사냥 대상
  전멸)이 재현되는지, 그때 봇이 실제로 순찰을 도는지는 아직 미확인.

- **블디/웨지 SAIN 성격을 F12에서 따로 설정 가능하게 함, 기본값 GigaChad**
  (`BDPersonalityOverride`, `BDPersonalityConfig`)

  블랙디비전은 팩션 설정상 바닐라 기준으로도 괴물급으로 설계된 팩션인데,
  SAIN 쪽 성격 배정은 이걸 전혀 반영하지 않고 있었습니다. `BlackDivSainRegistrations
  .cs`는 애초에 성격을 지정하는 필드가 없고, 블디의 커스텀 WildSpawnType
  (848420~848426)은 어떤 성격의 `AllowedTypes`에도, SAIN의 보스 성격 고정
  딕셔너리(`PERS_BOSSES`)에도 없습니다(둘 다 바닐라 `WildSpawnType` 기준).
  블디는 `IsPMC`도 아니라서(그건 pmcBEAR/pmcUSEC만 해당) PMC용 33% Chad
  폴백도 못 탐. 결과적으로 `PersonalityDictionary.GetPersonality`가 끝까지
  다 실패하고 매 스폰마다 대부분 `Normal`, 스폰당 3% 확률로만 우연히
  GigaChad가 걸리는 상태였습니다.

  1차 수정은 6종 전부 GigaChad로 무조건 고정하는 것이었으나, 웨지(보스)는
  따로 관리하고 싶다는 요청에 따라 **블디 5종(리드/어썰트/브리처/서포트/
  레이더)과 웨지를 별도 설정으로 분리**했습니다. 분리 전에 웨지가 이미
  특별한 AI를 갖고 있는지 먼저 확인했는데, **없었습니다** — 쇄빙선 레포의
  `docs/BOSSWEDGE-AI-REPORT.md`에 나오는 "BossWedge"는 레테일 바닐라의
  네이티브 보스 브레인(`ABossLogic` 파생)을 쇄빙선이 자기들 커스텀 보스
  설계 참고용으로 디컴파일해서 정리한 문서일 뿐, 블디의 웨지와는 이름만
  같고 완전히 무관합니다(이 레포 어디에도 `ABossLogic`을 참조하는 코드
  없음). 지금 웨지는 나머지 5종과 똑같이 SAIN 파이프라인만 탑니다 — 분리는
  "지금 다른 게 있어서"가 아니라 "나중에 따로 튜닝하고 싶을 때를 위해"
  해둔 것입니다.

  설정값은 BepInEx F12 메뉴 → "SAIN Personality" 섹션에 "Black Division
  Personality"(웨지 제외 5종) / "Wedge Personality" 둘로 노출됩니다. 각각
  기본값 GigaChad. **사용자가 SAIN 프리셋에서 그 성격 자체를 꺼뒀으면
  (`Personality Enabled` 끄기) 강제 안 하고 원래 계산 결과를 그대로
  둡니다** — SAIN이 원래 그 성격을 아무한테도 안 주게 확인하는 것과 같은
  플래그(`Assignment.Enabled`)를 봅니다.

  `SAINBotInfoClass.GetPersonality`를 postfix로 가로채는 방식은 그대로
  유지 — 봇 스폰 시점 생성자와 F12 프리셋 실시간 변경(`UpdatePresetSettings
  -> ConfigureBot`) 양쪽이 전부 거쳐가는 단일 지점이라, 라이드 중 프리셋이
  바뀌어도 설정이 유지됩니다.

  설정 바인딩(`BDPersonalityConfig.Bind`)은 `Plugin.Awake()` 안에 인라인으로
  안 넣고 별도 클래스로 뺐습니다. Mono에서는 메서드가 처음 JIT될 때 그
  메서드 본문의 모든 타입 토큰을 해석하려고 시도하기 때문에, `Config.Bind
  <EPersonality>(...)`를 `Awake()` 안에 직접 써두면 그 주변에 "SAIN 있으면"
  런타임 체크를 감싸도 소용없이 SAIN 미설치 시 `Awake()` 자체가 깨집니다.
  이 레포의 다른 SAIN 연동 코드가 전부 별도 파일로 빠져있는 것과 같은
  이유입니다.

  검증: 코드 레벨로는 확정. 실전 라이드에서 블디 봇의 활성 레이어/디버그
  오버레이에 설정한 성격이 실제로 찍히는지, F12 드롭다운이 정상적으로
  뜨는지는 아직 미확인.

- **실제 `dotnet build` 검증**

  지금까지의 검증은 전부 `dnfile`로 실제 dll의 멤버 시그니처를 하나하나
  대조하는 정적 검증이었는데, 이번엔 컨테이너에 .NET SDK를 직접 깔고
  진짜 컴파일까지 돌려봤습니다. `Prepatch.csproj`는 (Mono.Cecil을 NuGet에서
  받고 `UnityEngine.dll` 자리에 `MonoBehaviour`만 있는 최소 스텁을 넣어서)
  실제로 `BlackDiv.dll`이 나오는 것까지 확인했습니다. `Plugin.csproj`는
  이 환경에 컴파일된 `SAIN.dll`이 없어서(SAIN은 소스만 있고 빌드된 바이너리가
  없음) SAIN 타입을 직접 쓰는 4개 파일에서만 에러가 났는데, **그 에러 14개가
  전부 "SAIN을 못 찾는다"뿐이었고, 이번에 새로 짠 나머지 코드(`Vector3` 연산,
  `BDIdlePatrolLayer`/`BDIdlePatrolAction`, `Plugin.cs`의 `HuntManager`/
  `BrainManager` 호출)에는 에러가 하나도 없었습니다.** 즉 SAIN 의존 부분을
  뺀 나머지는 실제 컴파일러 기준으로 문법/API 오류가 없다는 게 확인됐습니다.
  SAIN.dll까지 직접 빌드해서 완전한 end-to-end 검증을 하는 건 이번엔 범위
  밖으로 남겨뒀습니다.

- **쇄빙선(Icebreaker) 맵 호환성 확인 — 블디가 IceCrew 크루로 편입됨**

  쇄빙선(`ManimalIcebreaker-zzap--Bootleg-`)은 `CrewBlackDivision`
  설정(기본값 켜짐)이 켜져 있으면 자기 크루 병력으로 블디의 848426(레이더)과
  848424(웨지)를 엔진룸/선미 구역에 직접 스폰시키고 Guard/Hold/Hunt
  임무(`IceCrewJobs`)를 붙입니다. 이 임무가 붙은 블디 봇은 비전투 중엔
  쇄빙선의 `IceCrewLayer`(68)/`IceHoldLayer`(105)/`IceRushLayer`(110)가
  담당하고, 전투가 붙으면(적이 보이거나 피격당하면) 이 세 레이어가 즉시
  물러나서 SAIN 콤뱃 레이어가 이어받습니다 — SAIN이 타겟을 놓치는 예외
  상황에서만 이번에 고친 블디 자체 `HuntTargetLayer`(+피격시 조준 fallback)와
  `BDIdlePatrolLayer`가 대신 받는 구조는 다른 맵과 동일합니다. 임무가 안 붙은
  일반 스폰 블디 봇(다른 맵, 또는 `CrewBlackDivision`을 꺼둔 경우)은 IceCrew
  레이어가 셋 다 조건 미달로 비활성이라 기존 스택 그대로 작동합니다.

---

## 호환성 참고 자료 (26/09/19)

코드 변경은 아니고, 다른 모드와 블디가 실제로 어떻게 상호작용하는지 소스
레벨로 확인한 내용입니다. `Cluade_For_spt`(`docs/SPT-4.1-PORTING-KB.md`
3.13절)에도 재사용 가능한 형태로 정리해뒀습니다.

### ORBIT 켤지 말지 — 장단점

블디는 ORBIT의 `TakeOverBlackDivision` 설정 대상입니다(기본값 꺼짐). 실제
레이어 코드(`Orbit/Brain/OrbitBrainLayer.cs`)까지 읽어서 확인한 내용:

- ORBIT 자체 레이어(`OrbitBrainLayer`)는 우선순위 **19**로, SAIN 콤뱃
  레이어(기본값 20)보다 낮게 **일부러** 설계돼 있습니다(코드 주석에 직접
  명시: "SAIN's combat layer (priority 20 > ORBIT 19) preempts"). 그래서
  **켜도 전투는 항상 SAIN이 그대로 담당** — ORBIT이 뺏어가는 건 그 아래
  비전투 구간, 즉 지금 블디 자체 `HuntTargetLayer`(10)/`BDIdlePatrolLayer`(5)가
  하던 일입니다.
- ORBIT은 BigBrain 레이어 자체는 빈 껍데기(`IdleAction`, 아무 것도 안 함)로
  두고, 실제 판단은 매 틱 여러 후보 행동에 점수를 매겨 최고점을 고르는
  자체 유틸리티 AI(`Agent`/`Squad`/`TaskScores`)로 따로 돌립니다. 스쿼드
  단위 목표 수립, 성격별 루팅 판단, 자체 막힘 감지(`Stuck.Soft/Hard`), 전투
  종료 후 15초/치료 중 60초 유예 같은 세밀한 SAIN 핸드오프까지 갖추고
  있어서, 블디 자체 폴백보다 훨씬 정교하고 "살아있는" 행동을 합니다.
- 단, ORBIT의 "헌트"는 **PvP 핫스팟(맵의 특정 구역)을 순찰하다 만나면
  싸우는** 개념이고, 블디 자체 헌트(`HuntManager.AddHuntRoles/AddHuntSides`,
  `HuntTargetLayer`)는 **유색크/베어 사이드를 특정해서 맵 어디에 있든
  추적하는** 개념입니다. ORBIT을 켜면 이 표적 추적형 헌트 자체가 사실상
  대체되어 사라집니다. ORBIT 개발자도 같은 이유로 "영구 헌트 레이어가
  핵심 매커니즘인 팩션"(ISB White Tusk 사례)은 아예 하드코딩으로 배제해둔
  전례가 있습니다 — 블디는 완전 배제는 아니고 토글로 열어뒀을 뿐, 같은
  우려가 적용되는 부류입니다.

| | ORBIT 켤 때 | ORBIT 끌 때(기본값) |
|---|---|---|
| 전투 | 동일 (SAIN, 항상 우선) | 동일 (SAIN, 항상 우선) |
| 비전투 행동 | ORBIT의 유틸리티 AI (루팅/핫스팟 순찰/스쿼드 목표, 더 다이나믹) | 블디 자체 순찰/보스-팔로워 뭉침 (단순, 정적) |
| "PMC를 사이드 단위로 끝까지 추적" | 사라짐 (핫스팟 순찰로 대체) | 유지 (블디 고유 정체성) |
| 성능 | Ghost Mode로 먼 봇 CPU 절약 (+45~100% 체감) | 없음 |
| 웨지 보스전 스폰 구성(랩스 전용 보스+레이더 호위) | 스폰 자체는 서버 설정이라 그대로 유지, 스폰 이후 행동만 ORBIT이 가져감 | 그대로 |

**요약**: 성능/범용 AI 품질을 우선하면 켜는 게 이득이지만, "블디는 PMC를
사이드 단위로 끝까지 쫓는 팩션"이라는 설계 의도를 유지하고 싶으면 꺼두는
쪽이 맞습니다. 지금 기본값(꺼짐)이 후자를 택한 상태입니다.

### 쇄빙선(Icebreaker)에서 블디+ORBIT이 동시에 켜져 있으면?

우선순위로 계산해보면 **ORBIT을 켜도 IceCrew 임무가 배정된 블디 봇한테는
사실상 영향이 없습니다.** IceCrew 세 레이어(`IceCrewLayer` 68,
`IceHoldLayer` 105, `IceRushLayer` 110)가 전부 ORBIT(19)보다 위에 있고,
쇄빙선 자체 스포너(`IcebreakerCrew.cs`)가 블디를 스폰시킬 때마다 항상
`IceCrewJobs.Assign(...)`으로 임무를 붙이기 때문에, 그 봇들은 비전투 중에도
계속 IceCrew 레이어가 이기고 ORBIT까지 순서가 안 내려옵니다(전투 중엔
당연히 SAIN이 이김). 즉 **쇄빙선에서 크루로 배치된 블디는 ORBIT을 켜든
안 켜든 사실상 동일하게 행동**합니다 — ORBIT은 "IceCrew 임무가 없는" 다른
맵의 일반 스폰 블디한테만 실질적인 영향을 줍니다.
(참고: 블디 서버 스폰 설정의 `huntMaps` 목록에 쇄빙선 같은 커스텀 맵 키가
기본으로 들어있는지는 확인 못 했습니다 — 지금 확인된 건 쇄빙선 자체
스포너를 통한 경로뿐입니다.)
