using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemCollectable : MonoBehaviour
{
    public static int balls;
    public static int coins;
    public static int diamonds;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Switch") || collision.gameObject.CompareTag("MultiColor"))
        {
            balls++;
            scoreText.text = "<sprite=0> " + balls;
        }
    }
}
