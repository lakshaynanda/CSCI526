using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollectable : MonoBehaviour
{
    public static int balls;
    
    [SerializeField] private Text scoreText;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Switch") || collision.gameObject.CompareTag("MultiColor")){
            balls++;
            scoreText.text = "Score: " + balls;
        }
    }
}
