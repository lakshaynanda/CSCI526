using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformColor : MonoBehaviour
{
    private float size = 0.0f;
    private Transform foreground;
    private float scale = 0.0005f;
    private bool expanding = false;

    private void Start()
    {
        foreground = transform.Find("Foreground");
    }
    
    private void Update()
    {
         if (expanding)
        {
            if (size < 1.02f)
            {
                size += scale;
                foreground.localScale = new Vector3(1f, size, 1f);
            }
            else
            {
                size = 0.0f;
                expanding = false;
            }
           
        }
        else
        {
            if (size == 0.0f)
            {
                expanding = true;
            }
        }
    }

    // private float currentValue = 0f;
    // public string[] colorListHex = { "#F4889A", "#F6E683", "#FFAF68", "#92D050"};
    // private Color randomColor;
    // private SpriteRenderer currentPlatformSprite;
    // [SerializeField] private Rigidbody2D rb;
    // private float timer = 0.0f;
    // private int seconds = 0;

    // // Start is called before the first frame update
    // private Transform colorChangingPlatform;

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //      if (collision.CompareTag("ColorChangingPlatform"))
    //     {
    //         ColorUtility.TryParseHtmlString(colorListHex[Random.Range(0, colorListHex.Length)], out randomColor);
    //         currentPlatformSprite = collision.gameObject.GetComponent<SpriteRenderer>();
    //     }
    // }
}
