using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    public static Vector2 Checkpoint;
    public static bool isRespawn = false;
    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Player"))
        {
            Checkpoint.x = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
            Checkpoint.y = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y+15;
            isRespawn = true;
        }
        
    }
}
