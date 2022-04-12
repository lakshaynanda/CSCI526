using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameRestart : MonoBehaviour
{
    [SerializeField] private Text scoreTextFinal;
    public void StartGame()
    {
        Player.health = 3;
        RespawnCheckpoint.isRespawn = false;
        Player.isLevelComplete = false;
        PlayerLives.hasTaken = false;
        scoreTextFinal.text = "Score: " + ItemCollectable.totalScore;
        if (SceneManager.GetActiveScene().name == "End Screen Tutorial")
        {
            SceneManager.LoadScene("Level 1");
        }
        else
        {
            SceneManager.LoadScene("Home");
        }
        ItemCollectable.totalScore = 0;
        ItemCollectable.currentLevelScore = 0;
    }
}
