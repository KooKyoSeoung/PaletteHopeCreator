# UI 설정 가이드

## 📋 개요
캐릭터 인벤토리와 가챠 시스템을 위한 UI를 Unity 에디터에서 설정하는 방법입니다.

## 🎨 필요한 UI 요소

### 1. 캐릭터 인벤토리 UI (CharacterInventoryUI)

#### 필요한 GameObject 구조:
```
Canvas (또는 기존 UI Canvas)
└── CharacterInventoryPanel
    ├── CharacterInventoryUI (스크립트)
    ├── Header
    │   ├── Title (TextMeshProUGUI)
    │   └── CloseButton (Button)
    ├── SelectedCharacterDisplay
    │   ├── SelectedCharacterImage (Image)
    │   └── SelectedCharacterNameText (TextMeshProUGUI)
    ├── CharacterListPanel
    │   └── CharacterListContent (ScrollView 또는 VerticalLayoutGroup)
    │       └── CharacterListParent (Transform) ← 여기에 슬롯들이 생성됨
    ├── GachaButton (Button)
    └── CharacterInfoPanel
        ├── CharacterInfoImage (Image)
        ├── CharacterNameText (TextMeshProUGUI)
        ├── CharacterRarityText (TextMeshProUGUI)
        ├── CharacterStatsText (TextMeshProUGUI)
        └── CloseButton (Button)
```

#### 설정 방법:
1. `CharacterInventoryPanel` GameObject 생성
2. `CharacterInventoryUI` 컴포넌트 추가
3. 인스펙터에서 다음 필드 할당:
   - `Character Panel`: CharacterInventoryPanel GameObject
   - `Character List Parent`: CharacterListParent Transform
   - `Character Slot Prefab`: 캐릭터 슬롯 프리팹 (아래 참조)
   - `Close Button`: 닫기 버튼
   - `Gacha Button`: 가챠 창 열기 버튼
   - `Selected Character Name Text`: 선택된 캐릭터 이름 텍스트
   - `Selected Character Image`: 선택된 캐릭터 이미지
   - `Character Info Panel`: 캐릭터 정보 패널
   - `Character Name Text`: 캐릭터 이름 텍스트
   - `Character Rarity Text`: 희귀도 텍스트
   - `Character Stats Text`: 능력치 텍스트
   - `Character Info Image`: 캐릭터 정보 이미지

### 2. 캐릭터 슬롯 프리팹 (CharacterSlotPrefab)

#### 필요한 GameObject 구조:
```
CharacterSlot (Prefab)
├── CharacterSlotUI (스크립트)
├── Background (Image)
├── CharacterImage (Image)
├── CharacterNameText (TextMeshProUGUI)
├── RarityText (TextMeshProUGUI)
├── SelectButton (Button)
├── InfoButton (Button)
└── SelectedIndicator (Image 또는 GameObject)
```

#### 프리팹 생성 방법:
1. Canvas에 임시 GameObject 생성 (`CharacterSlot`)
2. 구조대로 UI 요소 추가
3. `CharacterSlotUI` 컴포넌트 추가
4. 인스펙터에서 필드 할당:
   - `Character Image`: 캐릭터 이미지 Image
   - `Character Name Text`: 캐릭터 이름 TextMeshProUGUI
   - `Rarity Text`: 희귀도 TextMeshProUGUI
   - `Select Button`: 선택 버튼
   - `Info Button`: 정보 보기 버튼
   - `Selected Indicator`: 선택 표시 GameObject
5. 프리팹으로 저장 (`Assets/Prefabs/CharacterSlot.prefab`)

### 3. 가챠 UI (GachaUI)

#### 필요한 GameObject 구조:
```
GachaPanel
├── GachaUI (스크립트)
├── Header
│   ├── Title (TextMeshProUGUI)
│   └── CloseButton (Button)
├── InfoSection
│   ├── GachaPointsText (TextMeshProUGUI)
│   └── GachaCostText (TextMeshProUGUI)
├── GachaButton (Button)
├── BackToInventoryButton (Button)
├── ResultText (TextMeshProUGUI)
└── ResultPanel
    ├── ResultCharacterImage (Image)
    ├── ResultCharacterNameText (TextMeshProUGUI)
    ├── ResultRarityText (TextMeshProUGUI)
    └── ResultCloseButton (Button)
```

#### 설정 방법:
1. `GachaPanel` GameObject 생성
2. `GachaUI` 컴포넌트 추가
3. 인스펙터에서 다음 필드 할당:
   - `Gacha Panel`: GachaPanel GameObject
   - `Gacha Button`: 가챠 실행 버튼
   - `Close Button`: 닫기 버튼
   - `Back To Inventory Button`: 인벤토리로 돌아가기 버튼
   - `Gacha Points Text`: 가챠 포인트 표시 텍스트
   - `Gacha Cost Text`: 가챠 비용 표시 텍스트
   - `Result Text`: 결과 메시지 텍스트
   - `Result Panel`: 결과 패널
   - `Result Character Image`: 결과 캐릭터 이미지
   - `Result Character Name Text`: 결과 캐릭터 이름 텍스트
   - `Result Rarity Text`: 결과 희귀도 텍스트
   - `Result Close Button`: 결과 패널 닫기 버튼

## 🎮 사용 방법

### 캐릭터 인벤토리 열기
```csharp
CharacterInventoryUI inventoryUI = FindObjectOfType<CharacterInventoryUI>();
if (inventoryUI != null)
    inventoryUI.OpenPanel();
```

### 가챠 창 열기
```csharp
GachaUI gachaUI = FindObjectOfType<GachaUI>();
if (gachaUI != null)
    gachaUI.OpenPanel();
```

## 💡 디자인 팁

1. **ScrollView 사용**: 캐릭터가 많을 경우 `CharacterListParent`를 ScrollView의 Content로 설정
2. **Grid Layout**: 캐릭터 슬롯을 그리드로 배치하려면 `GridLayoutGroup` 사용
3. **색상 설정**: 희귀도별로 다른 배경색 사용
4. **애니메이션**: 결과 패널에 페이드 인/아웃 효과 추가

## 🔧 추가 기능 구현 예시

### 버튼에 인벤토리 열기 연결
```csharp
Button openInventoryButton; // Unity 인스펙터에서 할당

void Start()
{
    openInventoryButton.onClick.AddListener(() => {
        CharacterInventoryUI inventoryUI = FindObjectOfType<CharacterInventoryUI>();
        if (inventoryUI != null)
            inventoryUI.OpenPanel();
    });
}
```

### 게임 시작 시 자동으로 인벤토리 열기
```csharp
void Start()
{
    CharacterInventoryUI inventoryUI = FindObjectOfType<CharacterInventoryUI>();
    if (inventoryUI != null)
        inventoryUI.OpenPanel();
}
```

## 📝 참고사항

- 모든 UI 요소는 Canvas 하위에 있어야 합니다
- TextMeshPro를 사용해야 텍스트가 제대로 표시됩니다
- 프리팹을 사용하면 유지보수가 쉬워집니다
- 버튼 클릭 이벤트는 인스펙터에서도 연결할 수 있습니다

