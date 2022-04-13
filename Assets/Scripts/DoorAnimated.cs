using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void OpenDoor() {
        animator.SetBool("Open", true);
    }
    public void CloseDoor() {
        animator.SetBool("Open", false);
    }
}
