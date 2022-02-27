using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class restart : MonoBehaviour
{
    public void StartGame() 
    {
        ItemCollectable.balls = 0;
        Player.health = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
