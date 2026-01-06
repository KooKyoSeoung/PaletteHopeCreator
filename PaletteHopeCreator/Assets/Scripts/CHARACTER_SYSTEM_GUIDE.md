# 캐릭터 시스템 사용 가이드

## 📋 개요
이 시스템은 가챠를 통해 캐릭터를 획득하고, 각 캐릭터의 고유한 능력치로 게임을 플레이할 수 있게 해줍니다.

## 🎮 주요 기능
1. **캐릭터 데이터 관리**: ScriptableObject를 사용한 캐릭터 정보 저장
2. **가챠 시스템**: 확률에 따른 캐릭터 획득
3. **보유 캐릭터 관리**: PlayerPrefs를 통한 영구 저장
4. **캐릭터 능력 적용**: 각 캐릭터의 고유 능력치 적용

## 🚀 설정 방법

### 1단계: 캐릭터 데이터베이스 생성
1. Unity 에디터에서 `Assets` 우클릭 → `Create` → `Game` → `Character Database`
2. 생성된 `Character Database`를 선택하고 `allCharacters` 리스트에 캐릭터 추가

### 2단계: 캐릭터 데이터 생성
각 캐릭터마다:
1. `Assets` 우클릭 → `Create` → `Game` → `Character Data`
2. 캐릭터 정보 입력:
   - **characterName**: 캐릭터 이름 (고유해야 함)
   - **rarity**: 희귀도 (Common, Rare, Epic, Legendary)
   - **characterSprite**: 캐릭터 기본 스프라이트
   - **animationSprites**: 애니메이션용 스프라이트 배열
   - **gravity**: 중력 값 (기본: -9.8)
   - **strength**: 점프 강도 (기본: 5)
   - **scoreMultiplier**: 점수 배율 (1.0 = 기본)
   - **extraJumps**: 추가 점프 횟수
   - **slowMotionMultiplier**: 슬로우 모션 효과 (1.0 = 기본)
   - **characterColor**: 캐릭터 색상

3. 생성한 캐릭터 데이터를 `Character Database`의 `allCharacters` 리스트에 추가

### 3단계: 씬에 시스템 추가
1. 빈 GameObject 생성 (`CharacterSystem`)
2. 다음 컴포넌트 추가:
   - `CharacterInventory` 컴포넌트
     - `Character Database` 필드에 2단계에서 만든 데이터베이스 할당
   - `GachaSystem` 컴포넌트
     - `Character Database` 필드에 동일한 데이터베이스 할당
     - `Gacha Cost`: 가챠 1회 가격 설정 (기본: 100)
     - 희귀도 확률 설정 (합계 100%가 되도록)

### 4단계: 테스트
- 콘솔에서 `GachaSystem.Instance.GiveTestGachaPoints(1000)` 호출로 테스트 포인트 지급
- `GachaSystem.Instance.PullGacha()` 호출로 가챠 뽑기 테스트

## 💻 코드 사용 예시

### 가챠 뽑기
```csharp
GachaSystem gacha = GachaSystem.Instance;
if (gacha.CanPullGacha())
{
    CharacterData newCharacter = gacha.PullGacha();
    if (newCharacter != null)
    {
        Debug.Log($"새 캐릭터 획득: {newCharacter.characterName}");
    }
}
```

### 보유 캐릭터 확인
```csharp
CharacterInventory inventory = CharacterInventory.Instance;
List<CharacterData> owned = inventory.GetOwnedCharacters();

foreach (CharacterData character in owned)
{
    Debug.Log($"보유 캐릭터: {character.characterName}");
}
```

### 캐릭터 선택
```csharp
CharacterInventory inventory = CharacterInventory.Instance;
inventory.SetSelectedCharacter("캐릭터이름");
```

### 가챠 포인트 확인/추가
```csharp
GachaSystem gacha = GachaSystem.Instance;
int points = gacha.GetGachaPoints();
gacha.AddGachaPoints(100); // 100 포인트 추가
```

## 📊 캐릭터 능력치 설명

### 기본 스탯
- **gravity**: 중력 값 (음수일수록 빠르게 떨어짐)
- **strength**: 점프 강도 (높을수록 높이 점프)

### 특수 능력
- **scoreMultiplier**: 점수 배율
  - 1.0 = 기본 점수
  - 2.0 = 2배 점수
  - 예: 1.5 = 1.5배 점수

- **extraJumps**: 추가 점프 횟수
  - 0 = 추가 점프 없음
  - 1 = 공중에서 1번 더 점프 가능
  - 2 = 공중에서 2번 더 점프 가능

- **slowMotionMultiplier**: 슬로우 모션 효과
  - 1.0 = 정상 속도
  - 0.5 = 절반 속도 (더 느림, 조작하기 쉬움)
  - 2.0 = 2배 속도 (더 빠름)

## 💾 데이터 저장
모든 데이터는 `PlayerPrefs`에 저장됩니다:
- 보유 캐릭터 목록: `OwnedCharacters`
- 선택된 캐릭터: `SelectedCharacter`
- 가챠 포인트: `GachaPoints`

## 🔧 확장 가능성
- 게임 종료 시 가챠 포인트 지급 시스템 추가
- 캐릭터 레벨 시스템 추가
- 캐릭터 스킬 시스템 추가
- 가챠 포인트 획득 UI 추가

