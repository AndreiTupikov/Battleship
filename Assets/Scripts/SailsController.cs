using UnityEngine;

public class SailsController : MonoBehaviour
{
    public Animator[] sails;
    private bool isActive = false;
    private void Update()
    {
        if (!isActive && gameObject.GetComponent<PlayerMovement>().isMoving)
        {
            isActive = true;
            foreach (var sail in sails) sail.SetTrigger("Activate");
        }
        if (isActive && !gameObject.GetComponent<PlayerMovement>().isMoving)
        {
            isActive = false;
            foreach (var sail in sails) sail.SetTrigger("Deactivate");
        }
    }
}
