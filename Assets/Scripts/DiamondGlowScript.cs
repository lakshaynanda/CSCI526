using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondGlowScript : MonoBehaviour
{
    private bool glow = true;
    private float count = 1.1f;

    void Update()
    {
        if (glow && count<1)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            glow = false;
            count = 1.1f;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            glow = true;
            count -= Time.deltaTime;
        }
    }
}
