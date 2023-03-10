using System.Linq;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject ship;
    private float rechargeTime;
    private float rightBoardReady;
    private float leftBoardReady;
    private Transform[] rightBoardGuns;
    private Transform[] leftBoardGuns;

    private void Awake()
    {
        rechargeTime = ship.GetComponent<ShipStatus>().rechargeTime;
        rightBoardGuns = ship.transform.Find("RightBoard").GetComponentsInChildren<Transform>().Where(c => c.name != "RightBoard").Where(c => c.name != "GunSprite").ToArray();
        leftBoardGuns = ship.transform.Find("LeftBoard").GetComponentsInChildren<Transform>().Where(c => c.name != "LeftBoard").Where(c => c.name != "GunSprite").ToArray();
    }

    private void Update()
    {
#if UNITY_EDITOR
        UseMouseInput();
#elif UNITY_ANDROID
        UseTouchScreenInput();
#endif
    }

    private void UseTouchScreenInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.transform.gameObject.name == "RightBoard" && rightBoardReady < Time.time)
                        {
                            rightBoardReady = Time.time + rechargeTime;
                            Shoot(rightBoardGuns);
                        }
                        else if (hit.transform.gameObject.name == "LeftBoard" && leftBoardReady < Time.time)
                        {
                            leftBoardReady = Time.time + rechargeTime;
                            Shoot(leftBoardGuns);
                        }
                    }
                }
            }
        }
    }

    private void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.name == "RightBoard" && rightBoardReady < Time.time)
                {
                    rightBoardReady = Time.time + rechargeTime;
                    Shoot(rightBoardGuns);
                }
                else if (hit.transform.gameObject.name == "LeftBoard" && leftBoardReady < Time.time)
                {
                    leftBoardReady = Time.time + rechargeTime;
                    Shoot(leftBoardGuns);
                }
            }
        }
    }

    private void Shoot(Transform[] guns)
    {
        foreach(var gun in guns)
        {
            gun.GetComponent<Shot>().prepared = true;
            gun.GetComponent<Shot>().delay = Random.Range(0, 0.5f);
        }
    }
}
