using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulticolourEffect : MonoBehaviour
{
    private bool scaled = false;
    private float count = 2f;

    void Update()
    {
        if (scaled)
        {
            if (count < 1)
            {
                scaled = false;
                count = 2f;
            }
            else
            {
                gameObject.transform.localScale -= new Vector3(0.0005f, 0.0005f, 1f);
                count -= Time.deltaTime;
            }
        }
        else
        {
            if (count < 1)
            {
                scaled = true;
                count = 2f;
            }
            else
            {
                gameObject.transform.localScale += new Vector3(0.0005f, 0.0005f, 1f);
                count -= Time.deltaTime;
            }
        }
    }
}
