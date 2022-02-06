using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 14f;
    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;

    public Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    public GameObject gameOverCanvas;
    public GameObject levelCompletedCanvas;
    [SerializeField] private LayerMask jumpableGround;


    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        coll = GetComponent<BoxCollider2D>();
        gameOverCanvas.SetActive(false);
        levelCompletedCanvas.SetActive(false);
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
            mySprite.color = otherSprite.color;
            Destroy(collidedObject.gameObject);
        }
        else if (collidedObject.gameObject.CompareTag("Finish"))
        {
            levelCompletedCanvas.SetActive(true);
            Debug.Log("Level Completed");
            Destroy(collidedObject.gameObject);
            Die();
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
            gameOverCanvas.SetActive(true);
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
            gameOverCanvas.SetActive(true);
            Debug.Log("Game Over");
            Die();
        }
        else if (collidedObject.gameObject.CompareTag("Platform"))
        {
            if (mySprite.color != otherSprite.color)
            {
                gameOverCanvas.SetActive(true);
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
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
