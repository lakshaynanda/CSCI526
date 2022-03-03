using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour
{
    //public string LevelToLoad;

    public void restartLevel()
    {
        //Load the level from LevelToLoad
        ItemCollectable.balls = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void nextLevel()
    {
        //Load the level from LevelToLoad
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
