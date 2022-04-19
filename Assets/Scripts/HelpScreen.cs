using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    public static void LoadScreen()
    {
        Debug.Log("In help");
        Time.timeScale = 0.0f;
        SceneManager.LoadScene("HelpScreen");
    }
}
