using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerElement;
    public static float secondsLeft;
    public const float dangerTimeThreshold = 30f;
    [SerializeField] bool takingAway = false;
    public Rigidbody2D rb;
    private Animator anim;
    public int countballs;
    private string deathMessage = "";
    [SerializeField] public TextMeshProUGUI playerText;
    public AudioSource deathSound;

    public static int[] levelTime = { 40, 60, 100, 120, 120, 120 };

    void Start()
    {
        if (Portal.portalHit)
        {
            secondsLeft = Portal.timeLeft;
            Portal.portalHit = false;
        }
        else if (RespawnCheckpoint.isRespawn)
        {
            secondsLeft = levelTime[SceneManager.GetActiveScene().buildIndex - 1] / 2;
        }
        else
            secondsLeft = levelTime[SceneManager.GetActiveScene().buildIndex - 1];
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
        yield return new WaitForSeconds(0.5f);
        secondsLeft -= 0.5f;
        if (secondsLeft > 0 && secondsLeft <= dangerTimeThreshold)
        {
            timerElement.enabled = false;
        }

        yield return new WaitForSeconds(0.5f);
        secondsLeft -= 0.5f;
        if (secondsLeft > 0 && secondsLeft <= dangerTimeThreshold)
        {
            timerElement.enabled = true;
        }

        if (secondsLeft <= 0)
        {
            deathMessage = "Time's up!";
            RespawnCheckpoint.isRespawn = false;
            //ItemCollectable.totalScore = 0;
            ItemCollectable.currentLevelScore = 0;
            Time.timeScale = 0.01f;
            StartCoroutine(freezeDeath());
            timerElement.enabled = true;
            StopCoroutine(TimerTake());
        }
        Portal.timeLeft = secondsLeft;
        timerElement.text = "<sprite=0> " + secondsLeft;
        takingAway = false;

    }

    private IEnumerator freezeDeath()
    {
        deathSound.Play();
        GameObject camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        Camera cam = camera.GetComponent<Camera>();
        cam.orthographicSize = 2;
        playerText.SetText(deathMessage);
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1.0f;
        Player.health--;
        playerText.SetText("");
        if (Player.health > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else
        {
            RespawnCheckpoint.isRespawn = false;
            SceneManager.LoadScene("End Screen");
        }
    }
}
