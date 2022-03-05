using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour
{
    public void restartLevel()
    {
        //Load the level from LevelToLoad
        resetValues();
        Player.health = 3;
        ItemCollectable.balls = 0;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Home");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = 120;

    }
}
