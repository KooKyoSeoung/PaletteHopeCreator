using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject characterPanel;
    public Transform characterListParent; // 캐릭터 슬롯들이 들어갈 부모
    public GameObject characterSlotPrefab; // 캐릭터 슬롯 프리팹
    public Button closeButton;
    public Button gachaButton; // 가챠 창 여는 버튼
    public TextMeshProUGUI selectedCharacterNameText;
    public Image selectedCharacterImage;

    [Header("Layout Settings")]
    [Tooltip("한 줄에 배치할 캐릭터 슬롯 개수")]
    public int columnsPerRow = 3;
    [Tooltip("슬롯 간 간격 (X, Y)")]
    public Vector2 spacing = new Vector2(160, 160);
    [Tooltip("슬롯 크기 (너비, 높이) - 0이면 자동 계산")]
    public Vector2 cellSize = Vector2.zero;

    private CharacterInventory inventory;
    private List<CharacterSlotUI> characterSlots = new List<CharacterSlotUI>();
    private CharacterData selectedCharacter;

    void Start()
    {
        inventory = CharacterInventory.Instance;
        
        // GridLayoutGroup 자동 설정
        SetupGridLayout();
        
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
        
        if (gachaButton != null)
            gachaButton.onClick.AddListener(OpenGachaPanel);

        if (characterPanel != null)
            characterPanel.SetActive(false);

        RefreshCharacterList();
        UpdateSelectedCharacterDisplay();
    }

    private void SetupGridLayout()
    {
        if (characterListParent == null)
            return;

        // GridLayoutGroup 컴포넌트 가져오기 또는 추가
        GridLayoutGroup gridLayout = characterListParent.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = characterListParent.gameObject.AddComponent<GridLayoutGroup>();
        }

        // GridLayoutGroup 설정
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columnsPerRow;
        gridLayout.spacing = spacing;
        gridLayout.childAlignment = TextAnchor.UpperLeft;
        gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;

        // Cell Size 설정
        if (cellSize != Vector2.zero)
        {
            gridLayout.cellSize = cellSize;
        }
        else
        {
            // 자동으로 계산 (Viewport 또는 ScrollView의 너비를 기준으로)
            RectTransform parentRect = characterListParent.GetComponent<RectTransform>();
            
            // ScrollView의 Viewport를 찾아서 너비 계산
            ScrollRect scrollRect = characterListParent.GetComponentInParent<ScrollRect>();
            if (scrollRect != null && scrollRect.viewport != null)
            {
                float viewportWidth = scrollRect.viewport.rect.width;
                float cellWidth = (viewportWidth - (spacing.x * (columnsPerRow - 1))) / columnsPerRow;
                gridLayout.cellSize = new Vector2(cellWidth, cellWidth * 1.2f);
            }
            else if (parentRect != null)
            {
                float parentWidth = parentRect.rect.width;
                float cellWidth = (parentWidth - (spacing.x * (columnsPerRow - 1))) / columnsPerRow;
                gridLayout.cellSize = new Vector2(cellWidth, cellWidth * 1.2f);
            }
        }

        // Content Size Fitter 추가 (ScrollView를 위해 필수!)
        ContentSizeFitter sizeFitter = characterListParent.GetComponent<ContentSizeFitter>();
        if (sizeFitter == null)
        {
            sizeFitter = characterListParent.gameObject.AddComponent<ContentSizeFitter>();
        }
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // RectTransform 설정 (ScrollView Content용)
        RectTransform rectTransform = characterListParent.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Anchor를 Top-Left로 설정 (위에서 시작)
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            // 위치 초기화
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void OpenPanel()
    {
        characterPanel.SetActive(true);
        RefreshCharacterList();
        UpdateSelectedCharacterDisplay();
    }

    public void ClosePanel()
    {
        if (characterPanel != null)
            characterPanel.SetActive(false);
    }

    private void RefreshCharacterList()
    {
        if (inventory == null || characterListParent == null || characterSlotPrefab == null)
            return;

        // 기존 슬롯 제거
        foreach (var slot in characterSlots)
        {
            if (slot != null && slot.gameObject != null)
                Destroy(slot.gameObject);
        }
        characterSlots.Clear();

        // 보유 캐릭터 목록 가져오기
        List<CharacterData> ownedCharacters = inventory.GetOwnedCharacters();
        CharacterData currentSelected = inventory.GetSelectedCharacter();

        // 각 캐릭터마다 슬롯 생성
        foreach (CharacterData character in ownedCharacters)
        {
            GameObject slotObj = Instantiate(characterSlotPrefab, characterListParent);
            CharacterSlotUI slot = slotObj.GetComponent<CharacterSlotUI>();
            
            if (slot != null)
            {
                slot.Setup(character, this, character == currentSelected);
                characterSlots.Add(slot);
            }
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        if (character == null || inventory == null)
            return;

        inventory.SetSelectedCharacter(character.characterName);
        
        // 플레이어에게 즉시 적용
        Player player = FindObjectOfType<Player>();
        if (player != null)
            player.ApplyCharacterData();

        selectedCharacter = character;
        UpdateSelectedCharacterDisplay();
        RefreshCharacterList(); // 선택 상태 업데이트를 위해 리스트 새로고침
    }

    private void UpdateSelectedCharacterDisplay()
    {
        if (inventory == null)
            return;

        CharacterData current = inventory.GetSelectedCharacter();
        
        if (current != null)
        {
            if (selectedCharacterNameText != null)
                selectedCharacterNameText.text = $"선택: {current.characterName}";

            if (selectedCharacterImage != null && current.characterSprite != null)
            {
                selectedCharacterImage.sprite = current.characterSprite;
                selectedCharacterImage.color = Color.white;
            }
        }
        else
        {
            if (selectedCharacterNameText != null)
                selectedCharacterNameText.text = "선택된 캐릭터 없음";

            if (selectedCharacterImage != null)
                selectedCharacterImage.color = new Color(1, 1, 1, 0);
        }
    }

    private string GetRarityText(CharacterRarity rarity)
    {
        switch (rarity)
        {
            case CharacterRarity.Common:
                return "일반";
            case CharacterRarity.Rare:
                return "레어";
            case CharacterRarity.Epic:
                return "에픽";
            case CharacterRarity.Legendary:
                return "레전더리";
            default:
                return "알 수 없음";
        }
    }

    private void OpenGachaPanel()
    {
        GachaUI gachaUI = FindObjectOfType<GachaUI>();
        if (gachaUI != null)
        {
            ClosePanel();
            gachaUI.OpenPanel();
        }
    }

    // 캐릭터 추가 시 UI 업데이트 (가챠에서 새 캐릭터를 얻었을 때 호출)
    public void OnCharacterAdded()
    {
        RefreshCharacterList();
    }
}

