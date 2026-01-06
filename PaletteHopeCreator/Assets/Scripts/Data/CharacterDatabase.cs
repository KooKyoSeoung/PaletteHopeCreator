using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Database", menuName = "Game/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> allCharacters = new List<CharacterData>();

    public CharacterData GetCharacterByName(string name)
    {
        return allCharacters.Find(c => c.characterName == name);
    }

    public List<CharacterData> GetCharactersByRarity(CharacterRarity rarity)
    {
        return allCharacters.FindAll(c => c.rarity == rarity);
    }

    public CharacterData GetRandomCharacter()
    {
        if (allCharacters.Count == 0) return null;
        return allCharacters[Random.Range(0, allCharacters.Count)];
    }

    public CharacterData GetRandomCharacterByRarity(CharacterRarity rarity)
    {
        var characters = GetCharactersByRarity(rarity);
        if (characters.Count == 0) return null;
        return characters[Random.Range(0, characters.Count)];
    }
}

