using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    public GameObject textDisplay;
    public int secondsLeft = 120;
    public bool takingAway = false;
    public Rigidbody2D rb;
    private Animator anim;
    public static int countballs;

    void Start()
    {
        countballs = ItemCollectable.balls;
        textDisplay.GetComponent<Text>().text = "Time: " + secondsLeft;
    }

    void Update()
    {
        if (takingAway == false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft <= 0) {
            ItemCollectable.balls = 0;
            // rb.bodyType = RigidbodyType2D.Static;
            // anim.SetTrigger("death");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        textDisplay.GetComponent<Text>().text = "Timer: " + secondsLeft;
        takingAway = false;

    }



}
