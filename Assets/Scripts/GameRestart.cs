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
        PlayerLives.hasTaken = false;
        scoreTextFinal.text = "Score: " + ItemCollectable.balls;
       if (SceneManager.GetActiveScene().name == "End Screen Tutorial")
        {
         SceneManager.LoadScene("Tutorial");
        }else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
   ItemCollectable.balls = 0;
}
}
