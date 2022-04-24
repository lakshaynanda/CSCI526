using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public static void displayText(GameObject gameObject, string text)
    {
        gameObject.GetComponentInChildren<TextMesh>().text = text;
    }
}
