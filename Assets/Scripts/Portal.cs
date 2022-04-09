using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //public static Vector2 Checkpoint;
    public static bool portalHit = false;

    [SerializeField] private float speed = 1f;

    [SerializeField] private AudioSource portalSound;

    void Update()
    {
        transform.Rotate(180f * speed * Time.deltaTime,0f ,0f);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Player"))
        {
            portalSound.Play();
            portalHit = true;
        }
        
    }
}
