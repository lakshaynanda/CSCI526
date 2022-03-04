using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameCompleted : MonoBehaviour
{
    [SerializeField] private Text gameOverScoreTextFinal;
    [SerializeField] private Text gameCompleteScoreTextFinal;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject GameCompletedCanvas;

    void Start()
    {
        if (Player.health == 0)
        {
            GameCompletedCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
        }
        else
        {
            GameOverCanvas.SetActive(false);
            GameCompletedCanvas.SetActive(true);
        }
        int score = 0;
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        gameOverScoreTextFinal.text = "Score: " + score;
        gameCompleteScoreTextFinal.text = "Score: " + score;
    }
}
