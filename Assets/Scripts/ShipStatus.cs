using UnityEngine;

public class ShipStatus : MonoBehaviour
{
    public float health;
    public GameObject hitAnim;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Bullet"))
        {
            Vector3 pos = trigger.transform.position;
            GameObject hit = Instantiate(hitAnim, pos, trigger.transform.rotation);
            Destroy(hit, 1f);
            Destroy(trigger.gameObject);
            health -= 10;
            if (health < 0) Destroy(gameObject);
        }
    }
}
