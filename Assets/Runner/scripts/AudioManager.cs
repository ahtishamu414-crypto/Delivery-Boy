using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("FX")]
    public AudioSource coinCollect;
    public AudioSource obstacleHit;
    public AudioSource enemyHit;
    public AudioSource fall;
    void Awake()
    {
        instance = this;
    }

    public void PlayCoinCollect()
    {
        coinCollect.Play();
    }
    public void PlayEnemyHit()
    {
        enemyHit.Play();
    }

    public void PlayObstacleHit()
    {
        obstacleHit.Play();
    }
    public void PlayFall()
    {
        fall.Play();
    }
}