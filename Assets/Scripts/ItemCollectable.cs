using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollectable : MonoBehaviour
{
    public static int balls;
    public static int coins;
     public static int diamonds;
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text diamondText;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Switch") || collision.gameObject.CompareTag("MultiColor")){
            balls++;
            scoreText.text = "Score: " + balls;
        }
        if(collision.gameObject.CompareTag("Coin")){
            coins++;
            coinsText.text = "Coins: " + coins;
        }
         if(collision.gameObject.CompareTag("Diamond")){
            diamonds++;
            diamondText.text = "Diamonds: " + diamonds;
        }
    }
}
