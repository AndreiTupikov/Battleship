using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatus : MonoBehaviour
{
    public float health;
    public float maxSpeed;
    public float rechargeTime;
    public float attackDistance;
    public GameObject hitAnim;
    public GameObject smokeCenter;
    public GameObject defeatScreen;
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
            hit.transform.localScale += new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), 0);
            Destroy(hit, 1f);
            if (Random.Range(0, 5) == 0) AddSmoke(pos);
            Destroy(trigger.gameObject);
        }
    }

    private void Defeat()
    {
        defeated = true;
        maxSpeed = 0;
        Destroy(gameObject, 10f);
        if (gameObject.CompareTag("Enemy"))
        {
            int score = int.Parse(GameObject.Find("Score").GetComponent<Text>().text.Split(' ')[1]) + 1;
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
            AddNewEnemy();
        } else if (gameObject.CompareTag("Player"))
        {
            defeatScreen.SetActive(true);
        }
    }

    private void AddSmoke(Vector3 pos)
    {
        Instantiate(smokeCenter, pos, Quaternion.Euler(0, 0, 0), transform);
    }

    private void AddNewEnemy()
    {
        GameObject.Find("Main Camera").GetComponent<EnemyGenerator>().count++;
    }
}
