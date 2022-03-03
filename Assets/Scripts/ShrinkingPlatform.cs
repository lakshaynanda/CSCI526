using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    private float scale = 0.04f;
    private bool shrinking = false;

    void Update()
    {
        if (shrinking)
        {
           scale -= 0.01f;
           transform.localScale = new Vector2(scale, this.transform.localScale.y);
        }
        else
        {
            scale += 0.01f;
            transform.localScale = new Vector2(scale, this.transform.localScale.y);
        }

        if (shrinking && scale <= 0.04f)
        {
            shrinking = false;
        }
        else if (!shrinking && scale >= 6f)
        {
            shrinking = true;
        }
    }
}
