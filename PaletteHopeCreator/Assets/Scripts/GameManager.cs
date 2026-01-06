using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TMP_Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public Button openInventoryButton;

    private int score;
    private List<Pipes> activePipes = new List<Pipes>();

    void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }

    void Start()
    {
        if (openInventoryButton != null)
        {
            openInventoryButton.onClick.AddListener(() => {
                CharacterInventoryUI inventoryUI = FindObjectOfType<CharacterInventoryUI>();
                
                inventoryUI.OpenPanel();
            });
        }
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        gameOver.SetActive(false);
        playButton.SetActive(false);
        openInventoryButton.gameObject.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        if (player != null)
            player.ApplyCharacterData();

        for (int i = activePipes.Count - 1; i >= 0; --i)
        {
            if (activePipes[i] != null)
                Destroy(activePipes[i].gameObject);
        }
        activePipes.Clear();
    }

    public void RegisterPipe(Pipes pipe)
    {
        if (pipe != null && !activePipes.Contains(pipe))
            activePipes.Add(pipe);
    }

    public void UnregisterPipe(Pipes pipe)
    {
        activePipes.Remove(pipe);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        openInventoryButton.gameObject.SetActive(true);

        GachaSystem gacha = GachaSystem.Instance;
        if (gacha != null)
        {
            gacha.GiveTestGachaPoints(score * 10);
        }

        Pause();
    }

    public void IncreaseScore()
    {
        float multiplier = player != null ? player.GetScoreMultiplier() : 1.0f;
        score += Mathf.RoundToInt(1 * multiplier);
        scoreText.text = score.ToString();
    }
}
