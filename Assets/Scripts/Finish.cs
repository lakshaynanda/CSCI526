using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
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
        Debug.Log("In finish");
        Debug.Log(ItemCollectable.balls);
        PlayerPrefs.SetInt("Score", ItemCollectable.balls);
        if(SceneManager.GetActiveScene().name == "Tutorial"){
        
        }else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        TimerCountdown.secondsLeft = 120;
    }
}
