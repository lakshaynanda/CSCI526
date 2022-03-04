using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Finish : MonoBehaviour
{
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Player")
        {
            CompleteLevel();
        }
    }
    private void CompleteLevel() 
    {
        PlayerPrefs.SetInt("Score", ItemCollectable.balls);
        if(SceneManager.GetActiveScene().name == "Tutorial"){
        
        }else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        TimerCountdown.secondsLeft = 120;
    }
}
