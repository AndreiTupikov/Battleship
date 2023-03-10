using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject ship;
    public int count;

    private void Start()
    {
        count = DataHolder.enemyCount;
    }

    private void Update()
    {
        if (count > 0)
        {
            count--;
            GenerateEnemy();
        }
    }

    private void GenerateEnemy()
    {
        float angle;
        GameObject enemy = Instantiate(ship, GetPosition(out angle), Quaternion.Euler(0, 0, angle));
        enemy.tag = "Enemy";
    }

    private Vector3 GetPosition(out float angle)
    {
        float x = 0, y = 0;
        if (GetRandomSign() > 0) x = 12 * GetRandomSign();
        else y = 7 * GetRandomSign();
        if (x == 0)
        {
            angle = y < 0 ? 1 : 180;
            x = Random.Range(0f, 9f);
        }
        else
        {
            angle = x < 0 ? 270 : 90;
            y = Random.Range(0f, 4f);
        }
        return new Vector3(x, y);
    }

    private int GetRandomSign()
    {
        return Random.Range(0, 2) == 1 ? 1 : -1;
    }
}
