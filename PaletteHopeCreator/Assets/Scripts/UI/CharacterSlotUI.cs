using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI rarityText;
    public Button selectButton;
    public GameObject selectedIndicator; // 선택된 캐릭터 표시 (선택사항)

    private CharacterData characterData;
    private CharacterInventoryUI inventoryUI;

    void Awake()
    {
        if (selectButton != null)
            selectButton.onClick.AddListener(OnSelectClicked);
    }

    public void Setup(CharacterData character, CharacterInventoryUI ui, bool isSelected)
    {
        characterData = character;
        inventoryUI = ui;

        UpdateDisplay(isSelected);
    }

    private void UpdateDisplay(bool isSelected)
    {
        if (characterData == null)
            return;

        // 캐릭터 이름
        if (characterNameText != null)
            characterNameText.text = characterData.characterName;

        // 희귀도
        if (rarityText != null)
        {
            rarityText.text = GetRarityText(characterData.rarity);
            rarityText.color = GetRarityColor(characterData.rarity);
        }

        // 캐릭터 이미지
        if (characterImage != null)
        {
            if (characterData.characterSprite != null)
            {
                characterImage.sprite = characterData.characterSprite;
                characterImage.color = characterData.characterColor;
            }
            else
            {
                characterImage.color = new Color(0, 0, 0, 0);
            }
        }

        // 선택 표시 (있으면)
        if (selectedIndicator != null)
            selectedIndicator.SetActive(isSelected);
    }

    private void OnSelectClicked()
    {
        if (inventoryUI != null && characterData != null)
            inventoryUI.SelectCharacter(characterData);
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
                return "";
        }
    }

    private Color GetRarityColor(CharacterRarity rarity)
    {
        switch (rarity)
        {
            case CharacterRarity.Common:
                return Color.white;
            case CharacterRarity.Rare:
                return new Color(0.2f, 0.6f, 1f); // 파란색
            case CharacterRarity.Epic:
                return new Color(0.8f, 0.2f, 1f); // 보라색
            case CharacterRarity.Legendary:
                return new Color(1f, 0.6f, 0f); // 주황색
            default:
                return Color.white;
        }
    }
}

