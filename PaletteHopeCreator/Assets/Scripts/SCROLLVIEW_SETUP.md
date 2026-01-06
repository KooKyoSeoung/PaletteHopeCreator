# ScrollView 설정 가이드

## 📋 문제: 캐릭터 리스트가 스크롤되지 않음

캐릭터 리스트가 ScrollView 안에 제대로 들어가지 않아 스크롤이 되지 않는 문제를 해결하는 방법입니다.

## ✅ 올바른 하이라키 구조

ScrollView가 제대로 작동하려면 다음과 같은 구조여야 합니다:

```
Canvas
└── CharacterInventoryPanel
    └── ScrollView (ScrollRect 컴포넌트 필요)
        ├── Scrollbar Vertical (선택사항)
        └── Viewport (Image + Mask 컴포넌트 필요)
            └── Content (여기에 CharacterListParent가 있어야 함!)
                └── CharacterListParent (GridLayoutGroup + ContentSizeFitter)
                    ├── CharacterSlot (1)
                    ├── CharacterSlot (2)
                    └── CharacterSlot (3)
                        ...
```

## 🔧 수정 방법

### 방법 1: 기존 구조 수정 (권장)

1. **하이라키 확인**
   - `CharacterListParent`가 어디에 있는지 확인
   - ScrollView 안에 있는지 확인

2. **ScrollView 생성/수정**
   
   **만약 ScrollView가 없다면:**
   - Hierarchy에서 `CharacterInventoryPanel` 선택
   - 우클릭 → UI → Scroll View
   - 이름을 `CharacterScrollView`로 변경

   **ScrollView가 이미 있다면:**
   - `CharacterScrollView` 선택
   - ScrollRect 컴포넌트 확인:
     - `Content` 필드가 비어있지 않은지 확인
     - `Viewport` 필드가 올바르게 설정되어 있는지 확인

3. **CharacterListParent 이동**
   
   **현재 위치 확인:**
   - `CharacterListParent`가 ScrollView 밖에 있다면:
     - `CharacterScrollView` → `Viewport` → `Content` 안으로 드래그
   
   **Content가 없다면:**
   - `Viewport` 선택
   - 우클릭 → UI → Image (또는 빈 GameObject)
   - 이름을 `Content`로 변경
   - `CharacterListParent`를 `Content`의 자식으로 이동

4. **ScrollRect 설정**
   
   `CharacterScrollView` 선택 후 인스펙터:
   - **Content**: `Content` GameObject 할당 (CharacterListParent의 부모)
   - **Viewport**: `Viewport` GameObject 할당
   - **Movement Type**: Elastic
   - **Scroll Sensitivity**: 20
   - **Vertical**: ✅ 체크
   - **Horizontal**: ❌ 체크 해제

5. **CharacterInventoryUI 스크립트 수정**
   
   `CharacterInventoryUI` 컴포넌트:
   - **Character List Parent** 필드에 `Content` 또는 `CharacterListParent` 할당
   - (CharacterListParent가 Content와 같은 경우)

### 방법 2: 완전히 새로 만들기

1. **기존 CharacterListParent 삭제** (또는 백업)

2. **ScrollView 생성**
   - `CharacterInventoryPanel` 선택
   - 우클릭 → UI → Scroll View
   - 이름: `CharacterScrollView`

3. **구조 확인**
   ```
   CharacterScrollView
   ├── Scrollbar Vertical (자동 생성)
   └── Viewport
       └── Content (자동 생성)
   ```

4. **Content 이름 변경**
   - `Content`를 `CharacterListParent`로 이름 변경
   - 또는 그대로 두고 스크립트에서 `Content`를 참조

5. **CharacterInventoryUI 설정**
   - **Character List Parent** 필드에 `Viewport` → `Content` (또는 `CharacterListParent`) 할당

## ⚠️ 중요한 설정 체크리스트

### Viewport 설정
- [ ] Viewport에 **Mask** 컴포넌트가 있는가?
- [ ] Viewport의 **RectTransform** Anchor가 Stretch-Stretch인가?
- [ ] Viewport가 ScrollView의 전체 영역을 차지하는가?

### Content (CharacterListParent) 설정
- [ ] Content에 **GridLayoutGroup** 컴포넌트가 있는가?
- [ ] Content에 **ContentSizeFitter** 컴포넌트가 있는가?
  - Vertical Fit: Preferred Size
- [ ] Content의 **RectTransform** Anchor가 Top-Left인가?
- [ ] Content의 Pivot이 (0.5, 1)인가? (위에서 시작)

### ScrollRect 설정
- [ ] Content 필드가 올바른 GameObject를 가리키는가?
- [ ] Viewport 필드가 올바른 GameObject를 가리키는가?
- [ ] Vertical 스크롤이 활성화되어 있는가?

## 🎯 빠른 확인 방법

1. 게임 실행 후 인벤토리 열기
2. 캐릭터가 3개 이상일 때
3. 마우스 휠로 스크롤 시도
4. 스크롤이 되면 ✅ 성공!

## 🐛 문제 해결

### 스크롤이 안 될 때:
1. Content의 높이가 Viewport보다 작은지 확인
   - Content가 Viewport보다 크면 스크롤 가능
2. ScrollRect의 Content 필드가 비어있는지 확인
3. Content의 ContentSizeFitter가 제대로 작동하는지 확인

### 캐릭터가 보이지 않을 때:
1. Viewport의 Mask 컴포넌트 확인
2. Content의 위치가 Viewport 안에 있는지 확인
3. CharacterListParent의 위치 확인

