using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemCollectable : MonoBehaviour
{
    public static int totalScore;
    public static int currentLevelScore;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Switch"))
        {
            // totalScore++;
            // currentLevelScore++;
        } else if (collision.gameObject.CompareTag("MultiColor")) {
        }
        scoreText.text = "<sprite=0> " + totalScore;
    }
}
