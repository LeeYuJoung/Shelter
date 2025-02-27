using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    public bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector"))
        {
            isOpen = true;
            animator.SetTrigger("Open");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collector"))
        {
            isOpen = true;
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector"))
        {
            isOpen = false;
            animator.SetTrigger("Close");
        }
    }
}
