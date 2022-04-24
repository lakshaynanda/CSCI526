using UnityEngine;

public class ExpandingTrap : MonoBehaviour
{
    private float scale = 0.04f;
    private bool shrinking = false;

    void Update()
    {
         if (shrinking)
        {
           scale -= 0.01f;
           transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            scale += 0.01f;
            transform.localScale = new Vector2(scale, scale);
        }

        if (shrinking && scale <= 0.2f)
        {
            shrinking = false;
        }
        else if (!shrinking && scale >= 3f)
        {
            shrinking = true;
        }
    }
}
