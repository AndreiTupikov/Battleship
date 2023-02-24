using UnityEngine;

public class SailsController : MonoBehaviour
{
    public bool isMoving = false;
    private Animator[] sails;
    private bool sailsUp = false;
    private void Awake()
    {
        sails = transform.Find("Sails").GetComponentsInChildren<Animator>();
    }
    private void Update()
    {
        if (isMoving && !sailsUp)
        {
            sailsUp = true;
            foreach (var sail in sails) sail.SetFloat("Speed", 1f);
        }
        else if (!isMoving && sailsUp)
        {
            sailsUp = false;
            foreach (var sail in sails) sail.SetFloat("Speed", 0f);
        }
    }
}
