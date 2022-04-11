using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject lowVolumeElement;
    [SerializeField] GameObject highVolumeElement;
    [SerializeField] GameObject muteElement;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        if(AudioListener.volume == 0)
        {
            Mute();
        }
        else if(AudioListener.volume == 0.5f)
        {
            ReduceVolume();
        }
        else
        {
            IncreaseVolume();
        }
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
        AudioListener.volume = 0;
    }

    public void ReduceVolume()
    {
        lowVolumeElement.SetActive(true);
        highVolumeElement.SetActive(false);
        muteElement.SetActive(false);
        AudioListener.volume = 0.5f;

    }

    public void IncreaseVolume()
    {
        lowVolumeElement.SetActive(false);
        highVolumeElement.SetActive(true);
        muteElement.SetActive(false);
        AudioListener.volume = 1;
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
