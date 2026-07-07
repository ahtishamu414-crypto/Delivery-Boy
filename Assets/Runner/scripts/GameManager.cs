// using UnityEngine;
// using TMPro;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
//     public int score = 0;
//     public TextMeshProUGUI scoreText;
//     public GameObject gameOverPanel;

//     void Awake()
//     {
//         instance = this;
//     }

//     public void AddScore(int amount)
//     {
//         score += amount;
//         scoreText.text = "Coins: " + score;
//     }

//     public void GameOver()
//     {
//         StartCoroutine(ShowGameOver());
//     }

//     System.Collections.IEnumerator ShowGameOver()
//     {
//         yield return new WaitForSeconds(2f);
//         gameOverPanel.SetActive(true);
//     }
// }

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


    void Awake()
    {
        instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Coins: " + score;
    }

    // New method for health UI update
    public void HealthScore(int health)
    {
        healthText.text = "Health: " + health;
    }


   
}