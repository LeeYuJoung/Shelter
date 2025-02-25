using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Collector") && !isOpen))
        {
            isOpen = true;
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Collector")))
        {
            isOpen = false;
            animator.SetTrigger("Close");
        }
    }
}
