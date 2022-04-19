using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpResume : MonoBehaviour
{
    public void Resume()
    {
        Debug.Log("Resumed");
        Time.timeScale = 1f;
    }
}
