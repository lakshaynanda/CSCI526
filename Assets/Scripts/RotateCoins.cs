using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoins : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        transform.Rotate(0, 90 * speed * Time.deltaTime, 0f);
    }
}
