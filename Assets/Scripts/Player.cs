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
    [SerializeField] private Text scoreText;

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

    //Sticky limiter variables
    private float stickyTimer = 10;
    private Boolean stickyLimiter = false;
    private Boolean startStickyTimer = false;
    private Boolean startMulticolourTimer = false;
    private TextMeshProUGUI stickyPlatformText;
    private float seconds;

    [SerializeField] private LayerMask jumpableGround;
    public bool powerUpCollected = false;


    // Start is called before the first frame update
    void Start()
    {
        countballs = ItemCollectable.balls;
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        coll = GetComponent<BoxCollider2D>();
        gameOverCanvas.SetActive(false);
        levelCompletedCanvas.SetActive(false);
        healthText.text = "Health: " + health;
        stickyPlatformText = GameObject.Find("Sticky Text").GetComponent<TextMeshProUGUI>();
        multiColourText = GameObject.Find("Multicolor Text").GetComponent<TextMeshProUGUI>();
        sendLevelStartedAnalytics();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (stickyLimiter)
        {
            rb.velocity = new Vector2(dirX * 1f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(dirX * 5f, rb.velocity.y);
        }
        if (Input.GetButtonDown("Jump") && isGrounded() && !stickyLimiter)
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
        }
        if((startMulticolourTimer || startStickyTimer) && stickyTimer >= 0f)
        {
            stickyTimer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(stickyTimer % 60);
            if(startStickyTimer)
                stickyPlatformText.SetText("Low speed and no jump for " +  seconds + " secs");
            else if(startMulticolourTimer)
                multiColourText.SetText("Walk over any color for " + seconds + " secs");
        }
        healthText.text = "Health: " + health;
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        otherSprite = collidedObject.gameObject.GetComponent<SpriteRenderer>();
        if (collidedObject.gameObject.CompareTag("Switch"))
        {
            if (mySprite.color != Color.black)
            {
                mySprite.color = otherSprite.color;
            }
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Finish"))
        {
            levelCompletedCanvas.SetActive(true);
            Scene scene = collidedObject.gameObject.scene;
            Debug.Log("Level Completed: " + scene.name);
            AnalyticsResult analyticsResult = Analytics.CustomEvent("Level Completed: " + scene.name);
            Debug.Log("analytics" + analyticsResult);
            sendLevelCompletedAnalytics();
            //Die2();
        }
        else if (collidedObject.gameObject.CompareTag("MultiColor"))
        {
            ItemCollectable.balls -= 5;
            scoreText.text = "Score: " + ItemCollectable.balls;
            mySprite.color = otherSprite.color;
            startMulticolourTimer = true;
            Destroy(collidedObject.gameObject);
            Invoke(nameof(ResetEffect), 10);
        } else if (collidedObject.gameObject.CompareTag("StickyLimiter"))
        {
            Destroy(collidedObject.gameObject);
            stickyLimiter = true;
            startStickyTimer = true;
            Debug.Log("sticyTimer : " + startStickyTimer);
            Invoke(nameof(stopStickyEffect), 10);
        } 
    }

    private void stopStickyEffect()
    {
        stickyLimiter = false;
        startStickyTimer = false;
        stickyTimer = 10f;
        stickyPlatformText.SetText("");
    }

    private void OnCollisionEnter2D(Collision2D collidedObject)
    {
        checkColorMatch(collidedObject);
        checkTrapCollision(collidedObject);
    }

    private void checkTrapCollision(Collision2D collidedObject)
    {
        Scene scene = collidedObject.gameObject.scene;
        if (collidedObject.gameObject.CompareTag("Trap"))
        {
            triggerPlayerDeathEvent(collidedObject.gameObject.name);
            // gameOverCanvas.SetActive(true);
            Debug.Log("Game Over");
            Die();
        }
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
            // gameOverCanvas.SetActive(true);
            Debug.Log("Game Over");
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Platform"))
        {
            // Debug.Log("Hello" + mySprite.color);
            if (mySprite.color != otherSprite.color && mySprite.color != Color.black)
            {
                Debug.Log("Correct" + mySprite.color);
                triggerPlayerDeathEvent(collidedObject.gameObject.name);
                // gameOverCanvas.SetActive(true);
                Debug.Log("Game Over");
                Die();
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

    }
    private void Die()
    {
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
            SceneManager.LoadScene("End Screen");
        }

        // anim.SetTrigger("death");
    }

    //private void CompletedLevel()
    //{
    //    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    //}

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    //IEnumerator LoadLevel(int levelIndex)
    //{
    //    transition.SetTrigger("Start");
    //    yield return new WaitForSeconds(transitionTime);
    //    SceneManager.LoadScene(levelIndex);
    //}
    //private void Die2()
    //{
    //    Invoke("CompletedLevel", .2f);
    //}

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
        AnalyticsEvent.Custom("powerUp", new Dictionary<string, object>
        {
            { "powerUpCollected", powerUpCollected },
            { "level", SceneManager.GetActiveScene().name}
        });
        AnalyticsEvent.Custom("timeLeftEvent", new Dictionary<string, object>
        {
           { "timeLeft", TimerCountdown.secondsLeft},
            { "level", SceneManager.GetActiveScene().name}
        });

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            AnalyticsEvent.Custom("gameEnded");
        };
        AnalyticsEvent.Custom("livesRemaining", new Dictionary<string, object>
        {
            { "health", health},
            { "level", SceneManager.GetActiveScene().name}
        });
    }

    public void sendLevelStartedAnalytics()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            AnalyticsEvent.Custom("gameStarted");
        };
        AnalyticsEvent.Custom("livesRemaining", new Dictionary<string, object>
        {
            { "health", health},
            { "level", SceneManager.GetActiveScene().name}
        });
    }

    // public void incrHealth() {
    //     if (health < 3 && ItemCollectable.balls > 5) {
    //         health++;
    //         ItemCollectable.balls -= 5;
    //     }
    // }
}
