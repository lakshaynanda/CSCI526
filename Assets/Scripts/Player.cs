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
    [SerializeField] public int numberOfStickyPlatforms;
    private int keepCount = 0;
    

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
        stickyTexts = GameObject.FindGameObjectsWithTag("Sticky Messages");
        stickyPlatformText = stickyTexts[0].GetComponent<TextMeshProUGUI>();
        MultiColourTexts = GameObject.FindGameObjectsWithTag("Multicolour Messages");
        multiColourText = MultiColourTexts[0].GetComponent<TextMeshProUGUI>();
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
        if ((startMulticolourTimer || startStickyTimer) && stickyTimer >= 0f)
        {
            stickyTimer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(stickyTimer % 60);
            if (startStickyTimer)
                stickyPlatformText.SetText("Low speed and no jump for " + seconds + " secs");
            else if (startMulticolourTimer)
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
            sendLevelCompletedAnalytics();
            isLevelComplete = true;
            levelCompletedCanvas.SetActive(true);
            rb.bodyType = RigidbodyType2D.Static;
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
            powerUpCollected = true;
        } else if (collidedObject.gameObject.CompareTag("StickyLimiter"))
        {
            Destroy(collidedObject.gameObject);
            stickyLimiter = true;
            startStickyTimer = true;
            //Debug.Log("Text Name : " + stickyPlatformText.transform.name);
            Invoke(nameof(stopStickyEffect), 10);
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
        checkTrapCollision(collidedObject);
    }

    private void checkTrapCollision(Collision2D collidedObject)
    {
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
        keepCountMulti++;
        if (keepCountMulti < numberOfMultiColours)
        {
            multiColourText = MultiColourTexts[keepCountMulti].GetComponent<TextMeshProUGUI>();
        }
    }
    private void Die()
    {
        Debug.Log(ItemCollectable.balls);
        Debug.Log(countballs); 
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

    // public void incrHealth() {
    //     if (health < 3 && ItemCollectable.balls > 5) {
    //         health++;
    //         ItemCollectable.balls -= 5;
    //     }
    // }
}
