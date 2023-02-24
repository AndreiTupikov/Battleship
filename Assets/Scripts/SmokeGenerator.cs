using UnityEngine;

public class SmokeGenerator : MonoBehaviour
{
    public GameObject smokeAnim;
    public float intensity;
    private float smokeTime = 0;
    private void FixedUpdate()
    {
        if (smokeTime < Time.time)
        {
            smokeTime = Time.time + 1 / intensity;
            GameObject s = Instantiate(smokeAnim, new Vector3(transform.position.x + Random.Range(-0.06f, 0.06f), transform.position.y + Random.Range(-0.06f, 0.06f), 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            Destroy(s, 2f);
        }
    }
}
