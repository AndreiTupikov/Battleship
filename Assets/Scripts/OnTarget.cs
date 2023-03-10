using UnityEngine;

public class OnTarget : MonoBehaviour
{
    public bool onTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) onTarget = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) onTarget = false;
    }
}
