using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public static void displayText(GameObject gameObject, string text)
    {
        //GameObject gameObject = Instantiate(floatingPoints, transform.position, Quaternion.identity);
        gameObject.GetComponentInChildren<TextMesh>().text = text;
        Destroy(gameObject, 1f);
    }
}
