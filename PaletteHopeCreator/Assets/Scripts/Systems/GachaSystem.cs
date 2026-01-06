using System.Collections.Generic;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    private static GachaSystem instance;
    public static GachaSystem Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GachaSystem>();
            return instance;
        }
    }

    private const string GACHA_POINT_KEY = "GachaPoints";
    
    [Header("Gacha Settings")]
    public int gachaCost = 100; // 가챠 1회 가격
    public CharacterDatabase characterDatabase;

    [Header("Rarity Rates")]
    [Range(0f, 100f)]
    public float commonRate = 70f;      // 일반 확률
    [Range(0f, 100f)]
    public float rareRate = 20f;        // 레어 확률
    [Range(0f, 100f)]
    public float epicRate = 8f;         // 에픽 확률
    [Range(0f, 100f)]
    public float legendaryRate = 2f;    // 레전더리 확률

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetGachaPoints()
    {
        return PlayerPrefs.GetInt(GACHA_POINT_KEY, 0);
    }

    public void AddGachaPoints(int amount)
    {
        int current = GetGachaPoints();
        PlayerPrefs.SetInt(GACHA_POINT_KEY, current + amount);
        PlayerPrefs.Save();
    }

    public void SpendGachaPoints(int amount)
    {
        int current = GetGachaPoints();
        PlayerPrefs.SetInt(GACHA_POINT_KEY, Mathf.Max(0, current - amount));
        PlayerPrefs.Save();
    }

    public bool CanPullGacha()
    {
        return GetGachaPoints() >= gachaCost;
    }

    public CharacterData PullGacha()
    {
        if (!CanPullGacha())
        {
            Debug.LogWarning("가챠 포인트가 부족합니다!");
            return null;
        }

        if (characterDatabase == null || characterDatabase.allCharacters.Count == 0)
        {
            Debug.LogError("캐릭터 데이터베이스가 없거나 비어있습니다!");
            return null;
        }

        SpendGachaPoints(gachaCost);

        // 확률에 따라 희귀도 결정
        CharacterRarity rarity = GetRandomRarity();
        
        // 해당 희귀도의 캐릭터 중 랜덤 선택
        CharacterData character = characterDatabase.GetRandomCharacterByRarity(rarity);

        // 만약 해당 희귀도의 캐릭터가 없으면 랜덤 캐릭터 선택
        if (character == null)
        {
            character = characterDatabase.GetRandomCharacter();
        }

        if (character != null)
        {
            // 보유 목록에 추가
            CharacterInventory inventory = CharacterInventory.Instance;
            if (inventory != null)
            {
                inventory.AddCharacter(character.characterName);
            }

            Debug.Log($"가챠 결과: {character.characterName} ({rarity})");
        }

        return character;
    }

    private CharacterRarity GetRandomRarity()
    {
        float random = Random.Range(0f, 100f);
        float cumulative = 0f;

        cumulative += legendaryRate;
        if (random < cumulative)
            return CharacterRarity.Legendary;

        cumulative += epicRate;
        if (random < cumulative)
            return CharacterRarity.Epic;

        cumulative += rareRate;
        if (random < cumulative)
            return CharacterRarity.Rare;

        return CharacterRarity.Common;
    }

    public void GiveTestGachaPoints(int amount = 1000)
    {
        AddGachaPoints(amount);
        Debug.Log($"가챠 포인트 {amount}개 지급됨. 현재 포인트: {GetGachaPoints()}");
    }
}

