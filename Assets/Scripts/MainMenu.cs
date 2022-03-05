using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Player.health = 3;
        SceneManager.LoadScene("Level 1");
    }

    public void PlayTutorialGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
