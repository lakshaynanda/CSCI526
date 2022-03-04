using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameCompleted : MonoBehaviour
{
    [SerializeField] private Text scoreTextFinal;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I am here");
        int score = 0;
        if (PlayerPrefs.HasKey("Score"))
        {
            Debug.Log("Inside if");
            score = PlayerPrefs.GetInt("Score");
        }
        scoreTextFinal.text = "Score: " + score;
    }
}
