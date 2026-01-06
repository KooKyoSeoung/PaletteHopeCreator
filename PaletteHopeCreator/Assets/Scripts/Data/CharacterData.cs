using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public CharacterRarity rarity;
    public Sprite characterSprite;
    public Sprite[] animationSprites;

    [Header("Stats")]
    public float gravity = -9.8f;
    public float strength = 5f;
    
    [Header("Special Abilities")]
    [Tooltip("점수 배율 (1.0 = 기본, 2.0 = 2배)")]
    public float scoreMultiplier = 1.0f;
    
    [Tooltip("슬로우 모션 효과 (1.0 = 기본, 0.5 = 절반 속도)")]
    public float slowMotionMultiplier = 1.0f;

    [Header("Visual")]
    [Tooltip("캐릭터 색상 (선택사항)")]
    public Color characterColor = Color.white;
}

public enum CharacterRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

