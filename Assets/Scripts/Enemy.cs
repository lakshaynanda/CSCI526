using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 16f;
    private Rigidbody2D rb;
    private bool followPlayer = false;
    private Vector2 movement;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        var dist = Vector3.Distance(player.position, transform.position);
        // Debug.Log("Value of followPlayer:" + followPlayer);
        // Debug.Log(dist);
        direction.Normalize();
        movement = direction;
        if(dist < 51){
            followPlayer = true;
        } else {
            followPlayer = false;
        }
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            MoveEnemy(movement);
        }
    }

    private void MoveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2) transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
