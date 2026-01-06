using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gachaPanel;
    public Button gachaButton;
    public Button closeButton;
    public TextMeshProUGUI gachaPointsText;
    public TextMeshProUGUI gachaCostText;

    [Header("Result Display")]
    public GameObject resultPanel;
    public Image resultCharacterImage;
    public TextMeshProUGUI resultCharacterNameText;
    public TextMeshProUGUI resultRarityText;

    private GachaSystem gachaSystem;
    private CharacterInventoryUI inventoryUI;

    void Start()
    {
        gachaSystem = GachaSystem.Instance;
        inventoryUI = FindObjectOfType<CharacterInventoryUI>();

        if (gachaButton != null)
            gachaButton.onClick.AddListener(OnGachaButtonClicked);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        if (gachaPanel != null)
            gachaPanel.SetActive(false);

        if (resultPanel != null)
            resultPanel.SetActive(false);

        UpdateGachaUI();
    }

    void Update()
    {
        // UI가 활성화되어 있을 때만 업데이트
        if (gachaPanel != null && gachaPanel.activeSelf)
        {
            UpdateGachaUI();
        }
    }

    public void OpenPanel()
    {
        if (gachaPanel != null)
        {
            gachaPanel.SetActive(true);
            UpdateGachaUI();
        }
    }

    public void ClosePanel()
    {
        if (gachaPanel != null)
            gachaPanel.SetActive(false);

        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    private void UpdateGachaUI()
    {
        if (gachaSystem == null)
            return;

        int points = gachaSystem.GetGachaPoints();
        int cost = gachaSystem.gachaCost;

        // 가챠 포인트 표시
        if (gachaPointsText != null)
            gachaPointsText.text = $"보유 포인트: {points}";

        // 가챠 비용 표시
        if (gachaCostText != null)
            gachaCostText.text = $"가챠 비용: {cost}";

        // 가챠 버튼 활성화/비활성화
        if (gachaButton != null)
        {
            bool canGacha = gachaSystem.CanPullGacha();
            gachaButton.interactable = canGacha;

            // 버튼 텍스트 업데이트 (선택사항)
            TextMeshProUGUI buttonText = gachaButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                if (canGacha)
                    buttonText.text = "가챠 뽑기";
                else
                    buttonText.text = $"포인트 부족 ({cost - points} 부족)";
            }
        }
    }

    private void OnGachaButtonClicked()
    {
        if (gachaSystem == null)
            return;

        if (!gachaSystem.CanPullGacha())
        {
            return;
        }

        // 가챠 실행 전에 보유 캐릭터 개수 확인
        CharacterInventory inventory = CharacterInventory.Instance;
        int ownedCountBefore = 0;
        if (inventory != null)
        {
            ownedCountBefore = inventory.GetOwnedCharacterCount();
        }
        
        // 가챠 실행
        CharacterData result = gachaSystem.PullGacha();

        if (result != null)
        {
            // 가챠 후 보유 캐릭터 개수 확인
            int ownedCountAfter = 0;
            bool isNewCharacter = false;
            
            if (inventory != null)
            {
                ownedCountAfter = inventory.GetOwnedCharacterCount();
                // 개수가 증가했으면 새로운 캐릭터
                isNewCharacter = ownedCountAfter > ownedCountBefore;
            }
            
            ShowGachaResult(result, isNewCharacter);
            
            // 인벤토리 UI 업데이트 (새 캐릭터 추가됨)
            if (inventoryUI != null)
                inventoryUI.OnCharacterAdded();
        }

        UpdateGachaUI();
    }

    private void ShowGachaResult(CharacterData character, bool isNewCharacter)
    {
        if (resultPanel == null || character == null)
            return;

        resultPanel.SetActive(true);

        // 캐릭터 이름
        if (resultCharacterNameText != null)
            resultCharacterNameText.text = character.characterName;

        // 희귀도
        if (resultRarityText != null)
        {
            resultRarityText.text = GetRarityText(character.rarity);
            resultRarityText.color = GetRarityColor(character.rarity);
        }

        // 캐릭터 이미지
        if (resultCharacterImage != null)
        {
            if (character.characterSprite != null)
            {
                resultCharacterImage.sprite = character.characterSprite;
                resultCharacterImage.color = character.characterColor;
            }
        }

        // 3초 후 자동으로 결과 패널 닫기 (선택사항 - 코루틴 사용)
        StartCoroutine(AutoCloseResultPanel(3f));
    }

    private System.Collections.IEnumerator AutoCloseResultPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (resultPanel != null && resultPanel.activeSelf)
        {
            resultPanel.SetActive(false);
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

