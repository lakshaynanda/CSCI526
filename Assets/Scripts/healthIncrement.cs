using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthIncrement : MonoBehaviour
{
    public bool hasTaken;
    public GameObject button1;
    [SerializeField] private Text scoreText;

    void Start() {
        hasTaken = false;
        button1 = GameObject.Find("HealthPlus");
        button1.SetActive(false);
    }

    void Update() {
        // hasTaken is being set to false after each death and level restart -> need to prevent this
        if (Player.health < 3 && ItemCollectable.balls >= 5 && hasTaken == false) {
            enableButton();
        }
    }

    public void incrHealth() {
        if (Player.health < 3 && ItemCollectable.balls >= 5 && hasTaken == false) {
            Player.health++;
            ItemCollectable.balls -= 5;
            scoreText.text = "Score: " + ItemCollectable.balls;
            hasTaken = true;
            removeButton();
        }
    }

    void enableButton() {
        button1.SetActive(true);   
    }
    void removeButton() {
        button1.SetActive(false);   
    }  

}
