using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    private static CharacterInventory instance;
    public static CharacterInventory Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CharacterInventory>();
            return instance;
        }
    }

    private const string OWNED_CHARACTERS_KEY = "OwnedCharacters";
    private const string SELECTED_CHARACTER_KEY = "SelectedCharacter";

    private HashSet<string> ownedCharacterNames = new HashSet<string>();

    public CharacterDatabase characterDatabase;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadOwnedCharacters();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadOwnedCharacters()
    {
        ownedCharacterNames.Clear();
        string saved = PlayerPrefs.GetString(OWNED_CHARACTERS_KEY, "");
        
        if (!string.IsNullOrEmpty(saved))
        {
            string[] names = saved.Split('|');
            foreach (string name in names)
            {
                if (!string.IsNullOrEmpty(name))
                    ownedCharacterNames.Add(name);
            }
        }
        else
        {
            if (characterDatabase != null && characterDatabase.allCharacters.Count > 0)
            {
                AddCharacter(characterDatabase.allCharacters[0].characterName);
            }
        }
    }

    public void SaveOwnedCharacters()
    {
        string names = string.Join("|", ownedCharacterNames.ToArray());
        PlayerPrefs.SetString(OWNED_CHARACTERS_KEY, names);
        PlayerPrefs.Save();
    }

    public void AddCharacter(string characterName)
    {
        if (!ownedCharacterNames.Contains(characterName))
        {
            ownedCharacterNames.Add(characterName);
            SaveOwnedCharacters();
        }
    }

    public bool HasCharacter(string characterName)
    {
        return ownedCharacterNames.Contains(characterName);
    }

    public List<CharacterData> GetOwnedCharacters()
    {
        List<CharacterData> owned = new List<CharacterData>();
        
        if (characterDatabase == null) return owned;

        foreach (string name in ownedCharacterNames)
        {
            CharacterData character = characterDatabase.GetCharacterByName(name);
            if (character != null)
                owned.Add(character);
        }

        return owned;
    }

    public CharacterData GetSelectedCharacter()
    {
        string selectedName = PlayerPrefs.GetString(SELECTED_CHARACTER_KEY, "");
        
        if (string.IsNullOrEmpty(selectedName) && characterDatabase != null && characterDatabase.allCharacters.Count > 0)
        {
            selectedName = characterDatabase.allCharacters[0].characterName;
            SetSelectedCharacter(selectedName);
        }

        if (characterDatabase == null) return null;
        return characterDatabase.GetCharacterByName(selectedName);
    }

    public void SetSelectedCharacter(string characterName)
    {
        if (HasCharacter(characterName))
        {
            PlayerPrefs.SetString(SELECTED_CHARACTER_KEY, characterName);
            PlayerPrefs.Save();
        }
    }

    public int GetOwnedCharacterCount()
    {
        return ownedCharacterNames.Count;
    }
}

