using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levelCompletedCanvas;
    [SerializeField] GameObject lowVolumeElement;
    [SerializeField] GameObject highVolumeElement;
    [SerializeField] GameObject muteElement;

    void Update()
    {
        if(!levelCompletedCanvas || (levelCompletedCanvas && !levelCompletedCanvas.activeSelf)) {
            if (!pauseMenu.activeSelf)
            {
                Debug.Log("Hello");
                if (Input.GetKey("p"))
                {
                    Debug.Log("Hello2");
                    Pause();
                }
            }
            else
            {
                if (Input.GetKey("r"))
                {
                    Reload();
                }

                else if (Input.GetKey("p"))
                {
                    Resume();
                }
                else if (Input.GetKey("q"))
                {
                    Quit();
                }
                else if (Input.GetKey("v"))
                {
                    Mute();
                }
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        IncreaseVolume();
        Time.timeScale = 0f;

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = 120;
        ItemCollectable.totalScore -= ItemCollectable.currentLevelScore;
        ItemCollectable.currentLevelScore = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene.name);
    }

    public void Mute()
    {
        lowVolumeElement.SetActive(false);
        highVolumeElement.SetActive(false);
        muteElement.SetActive(true);
    }

    public void ReduceVolume()
    {
        lowVolumeElement.SetActive(true);
        highVolumeElement.SetActive(false);
        muteElement.SetActive(false);
    }

    public void IncreaseVolume()
    {
        lowVolumeElement.SetActive(false);
        highVolumeElement.SetActive(true);
        muteElement.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = 120;
        ItemCollectable.currentLevelScore = 0;
        ItemCollectable.totalScore = 0;
        SceneManager.LoadScene(0);
    }
}
