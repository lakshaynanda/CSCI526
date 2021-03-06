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
    public static int health = 3;
    public static bool isLevelComplete = false;
    public static int highScore = 0;
    string highScoreKey = "HighScore";

    public AudioSource coinSound;
    public AudioSource deathSound;
    public AudioSource jumpSound;
    public AudioSource CheckpointSound;
    public AudioSource finishSound;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public SpriteRenderer platformSprite;
    [SerializeField] public TextMeshProUGUI playerText;

    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;
    private TextMeshProUGUI multiColourText;
    private Boolean changeColorToNextPlatform = false;
    public static int countballs;
    public Rigidbody2D rb;
    private BoxCollider2D coll;
    private CircleCollider2D saw;
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
    private string deathMessage="";

    [SerializeField] private GameObject floatingPoints;
    [SerializeField] public Image timerForeground;
    [SerializeField] public float UpdateTimerBarSpeed;
    [SerializeField] private LayerMask jumpableGround;
    public bool powerUpCollected = false;
    private Animator anim;
    private enum MovementState { idle, running, jumping };

    void Start()
    {
        countballs = ItemCollectable.totalScore;
        ItemCollectable.currentLevelScore = 0;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        coll = GetComponent<BoxCollider2D>();
        saw = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        gameOverCanvas.SetActive(false);
        if (levelCompletedCanvas)
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
        }
        if (Portal.portalHit)
        {
            GameObject.FindGameObjectsWithTag("Player")[0].transform.position = GameObject.FindGameObjectsWithTag("Portal Right")[0].transform.position;
        }
        else
        {
            RespawnCheckpoint.Checkpoint = temp;
        }
    }
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        UpdateAnimationState(dirX);

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
            timerForeground.fillAmount = ((seconds + 1) * 10) / 100;
            //StartCoroutine(ChangeTimerBar(((seconds) * 10) / 100));
        }

        healthText.text = "<sprite=0> " + health;
        // if (isGrounded() && mySprite.color == Color.white && !startMulticolourTimer)
        // {
        //     mySprite.color = Color.red;
        // }
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
        // if (mySprite.color == Color.white && changeColorToNextPlatform && isGrounded())
        // {
        //     changeColorToNextPlatform = false;
        //     mySprite.color = otherSprite.color;
        // }
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
            finishSound.Play();
            //sendLevelCompletedAnalytics();
            isLevelComplete = true;
            if (levelCompletedCanvas)
            {
                levelCompletedCanvas.SetActive(true);
            }
            rb.bodyType = RigidbodyType2D.Static;
            RespawnCheckpoint.isRespawn = false;
        }
        else if (collidedObject.gameObject.CompareTag("MultiColor"))
        {
            GameObject floatingText = Instantiate(floatingPoints, transform.position, Quaternion.identity);
            if (ItemCollectable.totalScore >= 10)
            {
                GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
                Destroy(parent);
                ItemCollectable.totalScore -= 10;
                ItemCollectable.currentLevelScore -= 10;
                scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
                mySprite.color = otherSprite.color;
                startMulticolourTimer = true;
                FloatingText.displayText(floatingText, "-10");
                Destroy(floatingText, 1f);
                Invoke(nameof(ResetEffect), 10);
                powerUpCollected = true;
            }
            else
            {
                FloatingText.displayText(floatingText, "Not enough Points!");
                Destroy(floatingText, 1f);
            }

        }
        else if (collidedObject.gameObject.CompareTag("Coin"))
        {
            GameObject floatingText = Instantiate(floatingPoints, transform.position, Quaternion.identity);
            ItemCollectable.totalScore += 5;
            ItemCollectable.currentLevelScore += 5;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            FloatingText.displayText(floatingText, "+5");
            Destroy(floatingText, 1f);
            coinSound.Play();
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Diamond"))
        {
            GameObject floatingText = Instantiate(floatingPoints, transform.position, Quaternion.identity);
            GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
            ItemCollectable.totalScore += 10;
            ItemCollectable.currentLevelScore += 10;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            FloatingText.displayText(floatingText, "+10");
            Destroy(floatingText, 1f);
            coinSound.Play();
            Destroy(parent);
        }
        else if (collidedObject.gameObject.CompareTag("StickyLimiter"))
        {
            GameObject parent = collidedObject.gameObject.transform.parent.gameObject;
            Destroy(parent);
            stickyLimiter = true;
            startStickyTimer = true;
            Invoke(nameof(stopStickyEffect), 10);
        }
        else if (collidedObject.gameObject.CompareTag("Portal Left"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else if (collidedObject.gameObject.CompareTag("Trap"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            deathMessage = "Killed by an obstacle!";
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Foreground"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("CheckPoint"))
        {
            CheckpointSound.Play();
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
    }

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
            deathMessage = "Into the abyss";
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Platform"))
        {
            if (mySprite.color != otherSprite.color && mySprite.color != Color.white)
            {
                triggerPlayerDeathEvent(collidedObject.gameObject.name);
                deathMessage = "Color Mismatch!";
                Die();
            }
            if (freeze == true)
            {
                mySprite.color = otherSprite.color;
                freeze = false;
            }
        }
        else if (collidedObject.gameObject.CompareTag("Enemy"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            deathMessage = "Killed by enemy";
            Die();
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
        if (isGrounded())
        {
            mySprite.color = otherSprite.color;
        }
        else
        {
            // changeColorToNextPlatform = true;
            // Invoke(nameof(ResetEffect), 10);
            mySprite.color = otherSprite.color;
        }
        startMulticolourTimer = false;
        stickyTimer = 10f;
    }
    private void Die()
    {
        Time.timeScale = 0.01f;
        StartCoroutine(freezeDeath());
        // ItemCollectable.totalScore = countballs;
        // ItemCollectable.currentLevelScore = countballs;
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
        PlayerPrefs.SetInt("Score", ItemCollectable.totalScore);
        int probableHighScore = ItemCollectable.totalScore;
        health--;
        playerText.SetText("");
        // rb.bodyType = RigidbodyType2D.Static;
        if (health > 0)
        {
            getHighScore(probableHighScore);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else
        {
            RespawnCheckpoint.isRespawn = false;
            // if (SceneManager.GetActiveScene().name == "Tutorial")
            // {
            //     SceneManager.LoadScene("End Screen Tutorial");
            // }
            // else
            // {
            getHighScore(probableHighScore);
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
        if (health < 3 && ItemCollectable.totalScore > 15)
        {
            health++;
            ItemCollectable.totalScore -= 15;
            ItemCollectable.currentLevelScore -= 15;
        }
    }
    public void sendLevelCompletedAnalytics()
    {
        AnalyticsEvent.Custom("scoreEvent", new Dictionary<string, object>
        {
            { "score", ItemCollectable.currentLevelScore },
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("powerUpEvent", new Dictionary<string, object>
        {
            { "powerUpCollected", powerUpCollected },
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("timeTakenEvent", new Dictionary<string, object>
        {
           { "timeTaken", TimerCountdown.levelTime[SceneManager.GetActiveScene().buildIndex - 1]-(TimerCountdown.secondsLeft)},
            { "level", SceneManager.GetActiveScene().name}
        });

        if (SceneManager.GetActiveScene().name == "Level 5")
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

    private void UpdateAnimationState(float dirX)
    {
        MovementState playerMovementState;

        if (dirX > 0f)
        {
            mySprite.flipX = false;
            playerMovementState = MovementState.running;
        }
        else if (dirX < 0f)
        {
            mySprite.flipX = true;
            playerMovementState = MovementState.running;
        }
        else
        {
            playerMovementState = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            playerMovementState = MovementState.jumping;
        }

        anim.SetInteger("state", (int)playerMovementState);
    }
}
