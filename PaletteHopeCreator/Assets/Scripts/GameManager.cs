using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TMP_Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;

    private int score;
    private List<Pipes> activePipes = new List<Pipes>();

    void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        gameOver.SetActive(false);
        playButton.SetActive(false);

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

        Pause();
    }

    public void IncreaseScore()
    {
        float multiplier = player != null ? player.GetScoreMultiplier() : 1.0f;
        score += Mathf.RoundToInt(1 * multiplier);
        scoreText.text = score.ToString();
    }
}
