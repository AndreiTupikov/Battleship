using System.Collections.Generic;
using UnityEngine;

public class ShipStatus : MonoBehaviour
{
    public float health;
    public float maxSpeed;
    public GameObject hitAnim;
    public GameObject smokeCenter;
    private bool defeated;

    private void Update()
    {
        if (health < 0 && !defeated) Defeat();
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Bullet"))
        {
            health -= 10;
            Vector3 pos = trigger.transform.position;
            GameObject hit = Instantiate(hitAnim, pos, trigger.transform.rotation);
            Destroy(hit, 1f);


            if (health == 70 || health == 40 || health == 20) AddSmoke(pos);

            Destroy(trigger.gameObject);
        }
    }
    private void Defeat()
    {
        defeated = true;
        maxSpeed = 0;
        Destroy(gameObject, 10f);
    }
    private void AddSmoke(Vector3 pos)
    {
        Instantiate(smokeCenter, pos, Quaternion.Euler(0, 0, 0), transform);
    }
}
