using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            PlayLevel0();
        }
        else if (Input.GetKeyDown("1"))
        {
            PlayLevel1();
        }
        else if (Input.GetKeyDown("2"))
        {
            PlayLevel2();
        }
        else if (Input.GetKeyDown("3"))
        {
            PlayLevel3();
        }
        else if (Input.GetKeyDown("4"))
        {
            PlayLevel4();
        }
        else if (Input.GetKeyDown("5"))
        {
            PlayLevel5();
        }
    }

    public void PlayLevel0()
    {
        SceneManager.LoadScene("Level 0");
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void PlayLevel2()
    {
        Player.health = 3;
        SceneManager.LoadScene("Level 2");
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
}
