using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public void Update()
    {
        if (!pauseMenu.activeSelf)
        {
            if (Input.GetKeyDown("n"))
            {
                nextLevel();
            }
            if (Input.GetKeyDown("r"))
            {
                restartLevel();
            }
        }
    }

    public void restartLevel()
    {
        //Load the level from LevelToLoad
        resetValues();
        Player.health = 3;
        ItemCollectable.totalScore = 0;
        ItemCollectable.currentLevelScore = 0;
        // if (SceneManager.GetActiveScene().name == "Tutorial")
        // {
        //     SceneManager.LoadScene("Home");
        // }
        // else
        // {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // }
    }

    public void goHome()
    {
        SceneManager.LoadScene("Home");

    }
    public void nextLevel()
    {
        //Load the level from LevelToLoad
        resetValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void resetValues()
    {
        RespawnCheckpoint.isRespawn = false;
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = TimerCountdown.levelTime[SceneManager.GetActiveScene().buildIndex-1];

    }
}
