using UnityEngine;

public class Shot : MonoBehaviour
{
    public bool prepared = false;
    public float delay;
    public GameObject bullet;
    private bool isShooting = false;
    private float shotTime;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponents<AudioSource>()[Random.Range(0, 3)];
    }

    private void Update()
    {
        if (prepared)
        {
            prepared = false;
            isShooting = true;
            shotTime = Time.time + delay;
        }
        if (isShooting && shotTime < Time.time)
        {
            isShooting = false;
            Instantiate(bullet, transform.position, transform.rotation);
            audioSource.Play();
        }
    }
}