using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using TMPro;


public class Player : MonoBehaviour
{
    private float speed = 7f;
    private float jumpForce = 14f;
    public static int health = 3;
    public static bool isLevelComplete = false;
    public static int highScore = 0;
    string highScoreKey = "HighScore";
    [SerializeField] private TextMeshProUGUI scoreText;

    public AudioSource coinSound;
    public AudioSource deathSound;
    public AudioSource jumpSound;

    //[SerializeField] private Text healthText;
    [SerializeField] private TextMeshProUGUI healthText;
    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;
    private TextMeshProUGUI multiColourText;
    [SerializeField] public SpriteRenderer platformSprite;
    public static int countballs;
    public Rigidbody2D rb;
    private BoxCollider2D coll;
    public GameObject gameOverCanvas;
    public GameObject levelCompletedCanvas;
    public int startTime = 10;
    public int endTime;

    private float stickyTimer = 10;
    private Boolean stickyLimiter = false;
    private Boolean startStickyTimer = false;
    private Boolean startMulticolourTimer = false;

    private float seconds;
    private bool freeze;

    [SerializeField] public Image timerForeground;
    [SerializeField] public float UpdateTimerBarSpeed;
    [SerializeField] private LayerMask jumpableGround;
    public bool powerUpCollected = false;

    void Start()
    {
        countballs = ItemCollectable.totalScore;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        coll = GetComponent<BoxCollider2D>();
        gameOverCanvas.SetActive(false);
        if(levelCompletedCanvas)
        {
            levelCompletedCanvas.SetActive(false);
        }
        healthText.text = "<sprite=0> " + health;
        freeze = true;
        sendLevelStartedAnalytics();
        Vector2 temp = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        if (RespawnCheckpoint.isRespawn)
        {
            GameObject.FindGameObjectsWithTag("Player")[0].transform.position = RespawnCheckpoint.Checkpoint;
            RespawnCheckpoint.isRespawn = false;
        }
        RespawnCheckpoint.Checkpoint = temp;
        if (Portal.portalHit)
        {
            GameObject.FindGameObjectsWithTag("Player")[0].transform.position = GameObject.FindGameObjectsWithTag("Portal Right")[0].transform.position;
            Portal.portalHit = false;
        }
    }

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (dirX > 0f)
        {
            mySprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            mySprite.flipX = true;
        }
        if (stickyLimiter)
        {
            rb.velocity = new Vector2(dirX * 1f, rb.velocity.y);
        }
        else
        {
            if (!isLevelComplete)
                rb.velocity = (freeze == true ? new Vector2(0f, rb.velocity.y) : new Vector2(dirX * 5f, rb.velocity.y));
        }
        if (Input.GetButtonDown("Jump") && isGrounded() && !stickyLimiter)
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
            jumpSound.Play();
        }
        if ((startMulticolourTimer || startStickyTimer) && stickyTimer >= 0f)
        {
            stickyTimer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(stickyTimer % 60);
            StartCoroutine(ChangeTimerBar(((seconds) * 10) / 100));
        }

        healthText.text = "<sprite=0> " + health;
    }

    private IEnumerator ChangeTimerBar(float time)
    {
        float newTime = timerForeground.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < UpdateTimerBarSpeed)
        {
            elapsedTime += Time.deltaTime;
            timerForeground.fillAmount = Mathf.Lerp(newTime, time, elapsedTime / UpdateTimerBarSpeed);
            yield return null;
        }

        timerForeground.fillAmount = time;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        otherSprite = collidedObject.gameObject.GetComponent<SpriteRenderer>();
        if (collidedObject.gameObject.CompareTag("Switch"))
        {
            if (mySprite.color != Color.white)
            {
                mySprite.color = otherSprite.color;
            }
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Finish"))
        {
            sendLevelCompletedAnalytics();
            isLevelComplete = true;
            if(levelCompletedCanvas)
            {
                levelCompletedCanvas.SetActive(true);
            }
            rb.bodyType = RigidbodyType2D.Static;
        }
        else if (collidedObject.gameObject.CompareTag("MultiColor"))
        {
            GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
            ItemCollectable.totalScore -= 5;
            ItemCollectable.currentLevelScore -= 5;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            mySprite.color = otherSprite.color;
            startMulticolourTimer = true;
            Destroy(parent);
            Invoke(nameof(ResetEffect), 9);
            powerUpCollected = true;
        }
        else if (collidedObject.gameObject.CompareTag("Coin"))
        {
            ItemCollectable.totalScore += 5;
            ItemCollectable.currentLevelScore += 5;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            coinSound.Play();
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Diamond"))
        {
            GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
            ItemCollectable.totalScore += 10;
            ItemCollectable.currentLevelScore += 10;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            coinSound.Play();
            Destroy(parent);
        }
        else if (collidedObject.gameObject.CompareTag("StickyLimiter"))
        {
            GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
            Destroy(parent);
            stickyLimiter = true;
            startStickyTimer = true;
            Invoke(nameof(stopStickyEffect), 9);
        }
        else if (collidedObject.gameObject.CompareTag("Portal Left"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else if (collidedObject.gameObject.CompareTag("Trap"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Foreground"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            Die();
        }
    }

    private void stopStickyEffect()
    {
        stickyLimiter = false;
        startStickyTimer = false;
        stickyTimer = 10f;
    }


    private void OnCollisionEnter2D(Collision2D collidedObject)
    {
        checkColorMatch(collidedObject);
        //checkTrapCollision(collidedObject);
    }

    //private void checkTrapCollision(Collision2D collidedObject)
    //{
    //    if (collidedObject.gameObject.CompareTag("Trap"))
    //    {
    //        triggerPlayerDeathEvent(collidedObject.gameObject.name);
    //        Die();
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collidedObject)
    {
        checkColorMatch(collidedObject);
    }

    private void checkColorMatch(Collision2D collidedObject)
    {
        otherSprite = collidedObject.gameObject.GetComponent<SpriteRenderer>();
        if (collidedObject.gameObject.CompareTag("Border"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            Debug.Log("Game Over");
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Platform"))
        {
            if (mySprite.color != otherSprite.color && mySprite.color != Color.white)
            {
                Debug.Log("Correct" + mySprite.color);
                triggerPlayerDeathEvent(collidedObject.gameObject.name);
                Debug.Log("Game Over");
                Die();
            }
            if (freeze == true)
            {
                mySprite.color = otherSprite.color;
                freeze = false;
            }
        }
    }
    private void triggerPlayerDeathEvent(String spriteName)
    {
        AnalyticsEvent.Custom("playerDeathEvent", new Dictionary<string, object>
        {
            {"location", spriteName},
            {"level", SceneManager.GetActiveScene().name}
        });
    }
    public void ResetEffect()
    {
        mySprite.color = otherSprite.color;
        startMulticolourTimer = false;
        stickyTimer = 10f;
    }
    private void Die()
    {
        deathSound.Play();
        PlayerPrefs.SetInt("Score", ItemCollectable.totalScore);
        int probableHighScore = ItemCollectable.totalScore;
        // ItemCollectable.totalScore = countballs;
        // ItemCollectable.currentLevelScore = countballs;
        health--;
        // rb.bodyType = RigidbodyType2D.Static;
        if (health > 0)
        {
            getHighScore(probableHighScore);
            Debug.Log(PlayerPrefs.GetInt(highScoreKey, 0));
            //TimerCountdown.secondsLeft = 60;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else
        {
            RespawnCheckpoint.isRespawn = false;
            TimerCountdown.secondsLeft = 120;
            // if (SceneManager.GetActiveScene().name == "Tutorial")
            // {
            //     SceneManager.LoadScene("End Screen Tutorial");
            // }
            // else
            // {
            getHighScore(probableHighScore);
            Debug.Log(PlayerPrefs.GetInt(highScoreKey, 0));
            SceneManager.LoadScene("End Screen");
            // }
        }
    }

    private void getHighScore(int probableHighScore)
    {
        if (probableHighScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, probableHighScore);
            PlayerPrefs.Save();
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void incrHealth()
    {
        if (health < 3 && ItemCollectable.totalScore > 5)
        {
            health++;
            ItemCollectable.totalScore -= 5;
            ItemCollectable.currentLevelScore -= 5;
        }
    }
    public void sendLevelCompletedAnalytics()
    {
        AnalyticsEvent.Custom("scoreEvent", new Dictionary<string, object>
        {
            { "score", ItemCollectable.totalScore },
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("powerUpEvent", new Dictionary<string, object>
        {
            { "powerUpCollected", powerUpCollected },
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("timeLeftEvent", new Dictionary<string, object>
        {
           { "timeLeft", TimerCountdown.secondsLeft},
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("timeTakenEvent", new Dictionary<string, object>
        {
           { "timeTaken", 120-(TimerCountdown.secondsLeft)},
            { "level", SceneManager.GetActiveScene().name}
        });

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            AnalyticsEvent.Custom("gameEndedEvent");
        };
        AnalyticsEvent.Custom("livesRemainingEvent", new Dictionary<string, object>
        {
            { "health", health},
            { "level", SceneManager.GetActiveScene().name},
            { "location", "end"}
        });
    }

    public void sendLevelStartedAnalytics()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            AnalyticsEvent.Custom("gameStartedEvent");
        };
        AnalyticsEvent.Custom("livesRemainingEvent", new Dictionary<string, object>
        {
            { "health", health},
            { "level", SceneManager.GetActiveScene().name},
            { "location", "start"}
        });
    }
}
