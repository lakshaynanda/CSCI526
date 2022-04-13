using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Key.KeyType keyType;
    [SerializeField] private DoorAnimated door;
    public Key.KeyType GetKeyType() {
        return keyType;
    }
    public void OpenDoor() {
        gameObject.SetActive(false);
    }
}
