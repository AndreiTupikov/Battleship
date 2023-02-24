using UnityEngine;

public class BulletFlight : MonoBehaviour
{
    public float speed;
    public float distance;
    public GameObject splash;
    private float extraDistance;
    private float angleCorrection;

    private void Awake()
    {
        extraDistance = Random.Range(0, distance/10);
        angleCorrection = Random.Range(-0.1f, 0.1f);
    }

    private void Update()
    {
        gameObject.transform.Translate(speed * Time.deltaTime * new Vector3(1, angleCorrection, 0));
        distance -= 0.02f;
        if (distance + extraDistance < 0)
        {
            GameObject s = Instantiate(splash, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(s, 1f);
            Destroy(gameObject);
        }
    }
}
