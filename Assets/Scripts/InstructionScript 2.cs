using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionScript : MonoBehaviour
{
    [SerializeField] private GameObject prevInstruction;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(prevInstruction);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
