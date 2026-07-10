
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Health")]
    public TextMeshProUGUI healthText;

    [Header("Game Over")]
    public GameOverPanel gameOverPanel;

    void Awake()
    {
        instance = this;
    }

    public void AddScore(int amount)
{
    score += amount;
    scoreText.text = score.ToString(); // just the number
}

public void HealthScore(int health)
{
    healthText.text = health + "%";
}

    public void GameOver()
    {
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
{
    yield return new WaitForSeconds(1.5f);
    Debug.Log($"gameOverPanel is null: {gameOverPanel == null}");
    gameOverPanel.Setup(score);
}

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}