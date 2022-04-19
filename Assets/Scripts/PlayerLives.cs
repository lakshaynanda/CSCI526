using UnityEngine;
using TMPro;

public class PlayerLives : MonoBehaviour
{
    public static bool hasTaken;
    [SerializeField] GameObject buyHealthElement;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start() {
        buyHealthElement = GameObject.Find("IncrHealth");
        buyHealthElement.SetActive(false);
    }

    void Update() {
        if (Player.health < 3 && ItemCollectable.totalScore >= 15 && hasTaken == false) {
            enableButton();
            if(Input.GetKeyDown(KeyCode.L))
                incrHealth();
        }

    }

    public void incrHealth() {
        if (Player.health < 3 && ItemCollectable.totalScore >= 15 && hasTaken == false) {
            Player.health++;
            ItemCollectable.totalScore -= 15;
            ItemCollectable.currentLevelScore -= 15;
            scoreText.text = "<sprite=0> " + ItemCollectable.totalScore;
            hasTaken = true;
            removeButton();
        }
    }

    void enableButton() {
        buyHealthElement.SetActive(true);
    }
    void removeButton() {
        buyHealthElement.SetActive(false);
    }  

}
