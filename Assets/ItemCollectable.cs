using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollectable : MonoBehaviour
{
    private int balls = 0;
    [SerializeField] private Text scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Switch")){
            balls++;
            scoreText.text = "Score: " + balls;
        }
    }
}
