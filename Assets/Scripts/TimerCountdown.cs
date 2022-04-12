using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerElement;
    public static int secondsLeft;
    [SerializeField] bool takingAway = false;
    public Rigidbody2D rb;
    private Animator anim;
    public int countballs;

    public static int[] levelTime = {100,100,120,120,120};

    void Start()
    {
        if(RespawnCheckpoint.isRespawn)
            {
            secondsLeft = levelTime[SceneManager.GetActiveScene().buildIndex-2]/2;
            RespawnCheckpoint.isRespawn = false;
            }
        else
            secondsLeft = levelTime[SceneManager.GetActiveScene().buildIndex-2];
        countballs = ItemCollectable.totalScore;
        timerElement.text = "<sprite=0> " + secondsLeft;
    }

    void Update()
    {
        if (Player.isLevelComplete)
        {
            StopCoroutine(TimerTake());
        }
        else
        {
            if (takingAway == false && secondsLeft > 0)
            {
                StartCoroutine(TimerTake());
            }
        }
    }

    public IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft <= 0)
        {
            RespawnCheckpoint.isRespawn = false;
            ItemCollectable.totalScore = 0;
            ItemCollectable.currentLevelScore = 0;
            StopCoroutine(TimerTake());
            secondsLeft = levelTime[SceneManager.GetActiveScene().buildIndex];
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        timerElement.text = "<sprite=0> " + secondsLeft;
        takingAway = false;

    }
}
