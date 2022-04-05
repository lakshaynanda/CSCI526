using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    public GameObject textDisplay;
    public static int secondsLeft = 80;
    public bool takingAway = false;
    public Rigidbody2D rb;
    private Animator anim;
    public int countballs;

    void Start()
    {
        countballs = ItemCollectable.balls;
        textDisplay.GetComponent<Text>().text = "Time: " + secondsLeft;
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
            ItemCollectable.balls = 0;
            StopCoroutine(TimerTake());
            secondsLeft = 120;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        textDisplay.GetComponent<Text>().text = "Timer: " + secondsLeft;
        takingAway = false;

    }
}
