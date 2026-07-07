using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("FX")]
    public AudioSource coinCollect;
    public AudioSource obstacleHit;
    void Awake()
    {
        instance = this;
    }

    public void PlayCoinCollect()
    {
        coinCollect.Play();
    }
    public void PlayObstacleHit()
    {
        obstacleHit.Play();
    }
}