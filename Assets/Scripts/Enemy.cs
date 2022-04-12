using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 75f;
    private Rigidbody2D rb;
    private bool followPlayer = false;
    private Vector2 movement;
    private SpriteRenderer enemySprite;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        var dist = Vector3.Distance(player.position, transform.position);
        if ((transform.position.x - player.position.x) > 0f)
        {
            enemySprite.flipX = true;
        }
        else if ((transform.position.x - player.position.x) < 0f)
        {
            enemySprite.flipX = false;
        }
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
