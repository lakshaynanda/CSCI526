using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerElement;
    public static int secondsLeft = 120;
    [SerializeField] bool takingAway = false;
    public Rigidbody2D rb;
    private Animator anim;
    public int countballs;

    void Start()
    {
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
            ItemCollectable.totalScore = 0;
            ItemCollectable.currentLevelScore = 0;
            StopCoroutine(TimerTake());
            secondsLeft = 120;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        timerElement.text = "<sprite=0> " + secondsLeft;
        takingAway = false;

    }
}
