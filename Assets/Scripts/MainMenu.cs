using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            PlayTutorialGame();
        }
        else if(Input.GetKeyDown("2")){
            PlayLevel2();
        }
        else if(Input.GetKeyDown("3")){
            PlayLevel3();
        }
        else if(Input.GetKeyDown("4")){
            PlayLevel4();
        }
        else if(Input.GetKeyDown("5")){
            PlayLevel5();
        }
    }
    public void PlayLevel2()
    {
        Player.health = 3;
        SceneManager.LoadScene("Level 2");
    }

    public void PlayLevel0()
    {
        SceneManager.LoadScene("Level 0");
    }
    public void PlayTutorialGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void PlayLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void PlayLevel5()
    {
        SceneManager.LoadScene("Level 5");
    }
    // public void PlayLevel6()
    // {
    //     SceneManager.LoadScene("Level 6");
    // }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
