using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public string yellowPlatform = "YellowPlatform";
    public string redPlatform = "RedPlatform";
    public string greenPlatform = "GreenPlatform";
    public string bluePlatform = "BluePlatform";

    public string blueSwitch = "BlueSwitch";
    public string yellowSwitch = "YellowSwitch";
    public string greenSwitch = "GreenSwitch";
    public string redSwitch = "RedSwitch";

    public string currentPlatform;

    public Color colorRed;
    public Color colorYellow;
    public Color colorGreen;
    public Color colorBlue;

    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        colorRed = Color.red;
        colorYellow = Color.yellow;
        colorGreen = Color.green;
        colorBlue = Color.blue;
        sr.color = colorRed;
        currentPlatform = redPlatform;

        obj = GameObject.FindGameObjectWithTag("Finish");

        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool flag = true;
        if (collision.tag == blueSwitch)
        {
            sr.color = colorBlue;
            currentPlatform = bluePlatform;
        }
        else if (collision.tag == redSwitch)
        {
            sr.color = colorRed;
            currentPlatform = redPlatform;
        }
        else if (collision.tag == yellowSwitch)
        {
            sr.color = colorYellow;
            currentPlatform = yellowPlatform;
        }
        else if (collision.tag == greenSwitch)
        {
            sr.color = colorGreen;
            currentPlatform = greenPlatform;
        } else
        {
            flag = false;
            if (collision.tag == currentPlatform)
            {
                //Debug.Log(collision.tag);
                //Debug.Log(currentPlatform);
            }
            else
            {
                obj.SetActive(true);
                Debug.Log("Game Over");
                Application.Quit();
            }
        }

        if(flag)
        {
            Destroy(collision.gameObject);
        }

        
    }
}
