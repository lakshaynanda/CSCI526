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
    public float speed = 7f;
    public float jumpForce = 14f;
    public float transitionTime = 1f;
    public static int health = 3;
    public static bool isLevelComplete = false;
    [SerializeField] private Text scoreText;

    [SerializeField] private Text coinsText;

    [SerializeField] private Text diamondText;
    [SerializeField] private Text healthText;
    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;
    private TextMeshProUGUI multiColourText;
    [SerializeField] public SpriteRenderer platformSprite;
    public static int countballs;
    public Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    public GameObject gameOverCanvas;
    public GameObject levelCompletedCanvas;
    public int startTime = 10;
    public int endTime;
    public Animator transition;
    private GameObject[] MultiColourTexts;
    [SerializeField] public int numberOfMultiColours;
    private int keepCountMulti = 0;

    private float stickyTimer = 10;
    private Boolean stickyLimiter = false;
    private Boolean startStickyTimer = false;
    private Boolean startMulticolourTimer = false;
    private GameObject[] stickyTexts;
    private TextMeshProUGUI stickyPlatformText;
    private float seconds;
    private bool freeze;
    [SerializeField] public int numberOfStickyPlatforms;
    private int keepCount = 0;


    [SerializeField] private LayerMask jumpableGround;
    public bool powerUpCollected = false;

    void Start()
    {
        countballs = ItemCollectable.balls;
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        coll = GetComponent<BoxCollider2D>();
        gameOverCanvas.SetActive(false);
        levelCompletedCanvas.SetActive(false);
        healthText.text = "Lives: " + health;
        freeze = true;
        if (numberOfStickyPlatforms > 0)
        {
            stickyTexts = GameObject.FindGameObjectsWithTag("Sticky Messages");
            stickyPlatformText = stickyTexts[0].GetComponent<TextMeshProUGUI>();
        }
        if (numberOfMultiColours > 0)
        {
            MultiColourTexts = GameObject.FindGameObjectsWithTag("Multicolour Messages");
            multiColourText = MultiColourTexts[0].GetComponent<TextMeshProUGUI>();
        }
        sendLevelStartedAnalytics();
    }

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
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
        }
        if ((startMulticolourTimer || startStickyTimer) && stickyTimer >= 0f)
        {
            stickyTimer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(stickyTimer % 60);
            if (startStickyTimer)
                stickyPlatformText.SetText("Low speed and no jump for " + seconds + " secs");
            else if (startMulticolourTimer)
                multiColourText.SetText("Walk over any color for " + seconds + " secs");
        }

        healthText.text = "Lives: " + health;
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
            levelCompletedCanvas.SetActive(true);
            rb.bodyType = RigidbodyType2D.Static;
        }
        else if (collidedObject.gameObject.CompareTag("MultiColor"))
        {
            ItemCollectable.balls -= 5;
            scoreText.text = "Score: " + ItemCollectable.balls;
            mySprite.color = otherSprite.color;
            startMulticolourTimer = true;
            Destroy(collidedObject.gameObject);
            Invoke(nameof(ResetEffect), 10);
            powerUpCollected = true;
        }
        else if (collidedObject.gameObject.CompareTag("Coin"))
        {
            ItemCollectable.balls += 5;
            scoreText.text = "Score: " + ItemCollectable.balls;
            coinsText.text = "Coins: " + ItemCollectable.coins;
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Diamond"))
        {
            ItemCollectable.balls += 10;
            scoreText.text = "Score: " + ItemCollectable.balls;
            diamondText.text = "Diamonds: " + ItemCollectable.diamonds;
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("StickyLimiter"))
        {
            Destroy(collidedObject.gameObject);
            stickyLimiter = true;
            startStickyTimer = true;
            Invoke(nameof(stopStickyEffect), 10);
        } else if (collidedObject.gameObject.CompareTag("Trap"))
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
        stickyPlatformText.SetText("");
        keepCount++;
        if (keepCount < numberOfStickyPlatforms)
        {
            stickyPlatformText = stickyTexts[keepCount].GetComponent<TextMeshProUGUI>();
        }
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
        multiColourText.SetText("");
        keepCountMulti++;
        if (keepCountMulti < numberOfMultiColours)
        {
            multiColourText = MultiColourTexts[keepCountMulti].GetComponent<TextMeshProUGUI>();
        }
    }
    private void Die()
    {
        PlayerPrefs.SetInt("Score", ItemCollectable.balls);
        ItemCollectable.balls = countballs;
        health--;
        rb.bodyType = RigidbodyType2D.Static;
        if (health > 0)
        {
            TimerCountdown.secondsLeft = 120;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("End Screen Tutorial");
            }
            else
            {
                SceneManager.LoadScene("End Screen");
            }
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void incrHealth()
    {
        if (health < 3 && ItemCollectable.balls > 5)
        {
            health++;
            ItemCollectable.balls -= 5;
        }
    }
    public void sendLevelCompletedAnalytics()
    {
        AnalyticsEvent.Custom("scoreEvent", new Dictionary<string, object>
        {
            { "score", ItemCollectable.balls },
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
