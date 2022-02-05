using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour
{
    //public string LevelToLoad;

    public void loadLevel()
    {
        //Load the level from LevelToLoad
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
