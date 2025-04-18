using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    private int score;
    private int highScore;

    public UnityEvent OnScoreUpdate;
    public UnityEvent OnHighScoreUpdated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        OnHighScoreUpdated?.Invoke();
        GameManager.GetInstance().OnGameStart += OnGameStart;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void IncrementScore()
    {
        score++;
        OnScoreUpdate?.Invoke();

        if (score > highScore)
        {
            highScore = score;
            OnHighScoreUpdated?.Invoke();
        }
    }

    public void SetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void OnGameStart()
    {
        score = 0;
        OnScoreUpdate?.Invoke();
    }
}
