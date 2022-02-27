using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;

public class Player : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 14f;
    public static int health = 3;
    [SerializeField] private Text healthText;
    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;
    public SpriteRenderer platformSprite;
    public static int countballs;
    public Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    public GameObject gameOverCanvas;
    public GameObject levelCompletedCanvas;
    public int startTime = 10;
    public int endTime;
    [SerializeField] private LayerMask jumpableGround;


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
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * 5f, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        otherSprite = collidedObject.gameObject.GetComponent<SpriteRenderer>();
        if (collidedObject.gameObject.CompareTag("Switch"))
        {
            if (mySprite.color != Color.black) {
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
            Die2();
        } else if (collidedObject.gameObject.CompareTag("MultiColor")) {
            mySprite.color = otherSprite.color;
            Destroy(collidedObject.gameObject);
            Invoke(nameof(ResetEffect), 10);
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
            AnalyticsResult analyticsResult = Analytics.CustomEvent("Player Death: " + collidedObject.gameObject.name);
            Debug.Log("analytics" + analyticsResult);
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
            AnalyticsResult analyticsResult = Analytics.CustomEvent("Player Death: " + collidedObject.gameObject.name);
            Debug.Log("analytics" + analyticsResult);
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
                AnalyticsResult analyticsResult = Analytics.CustomEvent("Player Death: " + collidedObject.gameObject.name);
                Debug.Log("analytics" + analyticsResult);
                // gameOverCanvas.SetActive(true);
                Debug.Log("Game Over");
                Die();
            }
            
        }
        //else if (collidedObject.gameObject.CompareTag("Finish"))
        //{
        //    Debug.Log("Level Completed");
        //    Destroy(collidedObject.gameObject);
        //    Application.Quit();
        //}
    }
    public void ResetEffect()
    {
        mySprite.color = otherSprite.color;
    }
    private void Die()
    {
        ItemCollectable.balls = countballs;
        health--;
        rb.bodyType = RigidbodyType2D.Static;
        if (health > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        } else {
            SceneManager.LoadScene("End Screen");
        }
        
        // anim.SetTrigger("death");
    }
    private void CompletedLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private void Die2()
    {
        Invoke("CompletedLevel", .2f);
    }
}
