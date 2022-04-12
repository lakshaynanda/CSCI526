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
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Pause();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }

                else if (Input.GetKeyDown(KeyCode.P))
                {
                    Resume();
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    Quit();
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    if(muteElement.activeSelf){
                        ReduceVolume();
                    }
                    else if(lowVolumeElement.activeSelf){
                        IncreaseVolume();
                    }
                    else if(highVolumeElement.activeSelf){
                        Mute();
                    }
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
        RespawnCheckpoint.isRespawn = false;
        Player.health = 3;
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = TimerCountdown.levelTime[SceneManager.GetActiveScene().buildIndex-2];
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
        RespawnCheckpoint.isRespawn = false;
        Time.timeScale = 1f;
        Player.health = 3;
        PlayerLives.hasTaken = false;
        Player.isLevelComplete = false;
        TimerCountdown.secondsLeft = TimerCountdown.levelTime[SceneManager.GetActiveScene().buildIndex-2];
        ItemCollectable.currentLevelScore = 0;
        ItemCollectable.totalScore = 0;
        SceneManager.LoadScene(0);
    }
}
