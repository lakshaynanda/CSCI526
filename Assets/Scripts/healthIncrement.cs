using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthIncrement : MonoBehaviour
{
    public bool hasTaken = false;
    public GameObject button1;
    [SerializeField] private Text scoreText;
    void Start() {
        incrHealth();
        button1 = GameObject.Find("HealthPlus");
        button1.SetActive(true);
    }
    void enableButton() {
        button1.SetActive(true);   
    }
    void removeButton() {
        button1.SetActive(false);   
    }  
    public void incrHealth() {
        // if (ItemCollectable.balls > 5) {
        //     button1.SetActive(true);
        // }
        if (Player.health < 3 && ItemCollectable.balls > 5 && hasTaken == false) {
            Player.health++;
            ItemCollectable.balls -= 5;
            scoreText.text = "Score: " + ItemCollectable.balls;
            hasTaken = true;
            removeButton();
        }
    }
}
