using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private const string BestTimeKey = "BestTime";

    public void Setup(float currentTime)
    {
        // Show current time
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        currentScoreText.text = $"Time: {minutes:00}:{seconds:00}";

        // Check for best time in PlayerPrefs
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, float.MaxValue);
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(BestTimeKey, bestTime);
        }

        // Show best time
        int bestMinutes = Mathf.FloorToInt(bestTime / 60f);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60f);
        bestScoreText.text = $"Best: {bestMinutes:00}:{bestSeconds:00}";

        // Set up button events
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(QuitGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
