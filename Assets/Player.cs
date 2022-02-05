using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 10f;
    public Color StartColor;
    private SpriteRenderer mySprite;
    private SpriteRenderer otherSprite;

    public Rigidbody2D rb;
    private Animator anim;
    public GameObject gameOverCanvas;
    public GameObject levelCompletedCanvas;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = StartColor;
        gameOverCanvas.SetActive(false);
        levelCompletedCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector3(0, jumpForce, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        otherSprite = collidedObject.gameObject.GetComponent<SpriteRenderer>();
        if (collidedObject.gameObject.CompareTag("Switch"))
        {
            mySprite.color = otherSprite.color;
            Destroy(collidedObject.gameObject);
        } else if(collidedObject.gameObject.CompareTag("Finish"))
        {
            levelCompletedCanvas.SetActive(true);
            Debug.Log("Level Completed");
            Destroy(collidedObject.gameObject);
            Application.Quit();
        }
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
}
