using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject helpMenu;
    public void Update()
    {
        if (!pauseMenu.activeSelf && !helpMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Home();
            }
        }
    }

    public void RestartLevel()
    {
        resetValues();
        Player.health = 3;
        ItemCollectable.currentLevelScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        resetValues();
        Player.health = 3;
        ItemCollectable.currentLevelScore = 0;
        ItemCollectable.totalScore = 0;
        SceneManager.LoadScene("Home");

    }
    public void NextLevel()
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
        TimerCountdown.secondsLeft = TimerCountdown.levelTime[SceneManager.GetActiveScene().buildIndex - 1];

    }
}
