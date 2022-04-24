using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //public static Vector2 Checkpoint;
    public static bool portalHit = false;

    [SerializeField] private float speed;
    public static float timeLeft;
    [SerializeField] private AudioSource portalSound;

    void Update()
    {
        transform.Rotate(90 * speed * Time.deltaTime, 0f, 0f);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Player"))
        {
            portalSound.Play();
            portalHit = true;
        }
        
    }
}
