using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private float currentSpeed = 1;
    private bool isMoving = true;
    private bool isOnFight = false;
    private float attackDistance;
    private float rechargeTime;
    private float rightBoardReady;
    private float leftBoardReady;
    private GameObject rightBoard;
    private GameObject leftBoard;
    private Transform[] rightBoardGuns;
    private Transform[] leftBoardGuns;

    private void Start()
    {
        attackDistance = gameObject.GetComponent<ShipStatus>().attackDistance;
        rechargeTime = gameObject.GetComponent<ShipStatus>().rechargeTime;
        player = GameObject.Find("PlayerShip");
        rightBoard = gameObject.transform.Find("RightBoard").gameObject;
        leftBoard = gameObject.transform.Find("LeftBoard").gameObject;
        rightBoardGuns = rightBoard.GetComponentsInChildren<Transform>().Where(c => c.name != "RightBoard").Where(c => c.name != "GunSprite").ToArray();
        leftBoardGuns = leftBoard.GetComponentsInChildren<Transform>().Where(c => c.name != "LeftBoard").Where(c => c.name != "GunSprite").ToArray();
    }

    private void Update()
    {
        if (!isOnFight)
        {
            MoveToScene(); return;
        }
        else if (gameObject.GetComponent<ShipStatus>().maxSpeed > 0)
        {
            Navigation();
            SpeedCorrection();
            MoveEnemy();
            Fire();
        }
    }

    private void Navigation()
    {
        var distance = GetDistanceToPlayer(transform);
        if (isMoving)
        {
            TurnEnemy(GetAngle(distance < attackDistance));
        }
    }

    private void TurnEnemy(float angle)
    {
        transform.Rotate(angle * Time.deltaTime * new Vector3(0, 0, 1));
    }

    private float GetAngle(bool onAttackDistance)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float newAngle = Vector2.SignedAngle(Vector3.up, direction);
        float oldAngle = transform.rotation.eulerAngles.z;
        float rotateAngle = newAngle - oldAngle;
        if (rotateAngle > 180) rotateAngle -= 360;
        else if (rotateAngle < -180) rotateAngle = 360 + newAngle - oldAngle;
        if (onAttackDistance)
        {
            if (rotateAngle < 0) rotateAngle += 120;
            else if (rotateAngle > 0) rotateAngle -= 120;
        }
        else
        {
            if (rotateAngle > 90) rotateAngle = 90;
            else if (rotateAngle < -90) rotateAngle = -90;
        }
        return rotateAngle;
    }

    private void SpeedCorrection()
    {
        float maxSpeed = gameObject.GetComponent<ShipStatus>().maxSpeed;
        if (currentSpeed > 0 && (!isMoving || maxSpeed == 0))
        {
            currentSpeed -= 0.01f;
            gameObject.GetComponent<SailsController>().isMoving = false;
        }
        else if (isMoving && currentSpeed < maxSpeed)
        {
            currentSpeed += 0.01f;
            gameObject.GetComponent<SailsController>().isMoving = true;
        }
        if (currentSpeed > 0 && currentSpeed < 0.0001f) currentSpeed = 0;

    }

    private void MoveToScene()
    {
        if (Math.Abs(transform.position.x) < 9 && Math.Abs(transform.position.y) < 4)
        {
            isOnFight = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SailsController>().isMoving = true;
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        transform.Translate(currentSpeed * Time.deltaTime * new Vector3(0, 1, 0));
    }

    private void Fire()
    {
        if (rightBoardReady < Time.time && rightBoard.GetComponent<OnTarget>().onTarget)
        {
            rightBoardReady = Time.time + rechargeTime;
            Shoot(rightBoardGuns);
        }
        else if (leftBoardReady < Time.time && leftBoard.GetComponent<OnTarget>().onTarget)
        {
            leftBoardReady = Time.time + rechargeTime;
            Shoot(leftBoardGuns);
        }
    }

    private void Shoot(Transform[] guns)
    {
        foreach (var gun in guns)
        {
            gun.GetComponent<Shot>().prepared = true;
            gun.GetComponent<Shot>().delay = UnityEngine.Random.Range(0, 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (GetDistanceToPlayer(transform) > GetDistanceToPlayer(collision.transform)) StartCoroutine("PassEnother");
        }
    }

    private IEnumerator PassEnother()
    {
        isMoving = false;
        yield return new WaitForSeconds(2f);
        isMoving = true;
    }

    private float GetDistanceToPlayer(Transform from)
    {
        return Vector2.Distance(from.position, player.transform.position);
    }
}
