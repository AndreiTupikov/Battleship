using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform button;
    private int moveTouchId = -1;
    private float currentSpeed = 0;
    private Vector3 center;
    private void Awake()
    {
        center = button.position;
    }
    private void Update()
    {
#if UNITY_EDITOR
        UseMouseInput();
#elif UNITY_ANDROID
        UseTouchScreenInput();
#endif
        SpeedCorrection();
        MovePlayer();
    }

    private void UseTouchScreenInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (moveTouchId < 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            if (hit.transform.gameObject.name == "MoveButton")
                            {
                                moveTouchId = touch.fingerId;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (touch.fingerId == moveTouchId)
                    {
                        MoveButton(touch.position);
                        if (touch.phase == TouchPhase.Ended)
                        {
                            button.position = center;
                            moveTouchId = -1;
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            moveTouchId = -1;
        }
    }

    private void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.name == "MoveButton")
                {
                    moveTouchId = 1;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && moveTouchId > 0)
        {
            moveTouchId = -1;
            button.position = center;
        }
        if (moveTouchId > 0)
        {
            MoveButton(Input.mousePosition);
        }
    }

    private void MoveButton(Vector3 pos)
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(pos) - center;
        newPos.z = 0;
        if (Math.Abs(newPos.x) > 0.75 || Math.Abs(newPos.y) > 0.75) newPos = newPos.normalized;
        button.position = center;
        button.Translate(newPos * 0.75f);
        if (gameObject.GetComponent<ShipStatus>().maxSpeed > 0) TurnPlayer(newPos);
    }

    private void TurnPlayer(Vector3 newPos)
    {
        float newAngle = Vector2.SignedAngle(Vector3.up, newPos);
        float oldAngle = transform.rotation.eulerAngles.z;
        float rotateAngle = newAngle - oldAngle;
        if (rotateAngle > 180) rotateAngle -= 360;
        else if (rotateAngle < -180) rotateAngle = 360 + newAngle - oldAngle;
        if (rotateAngle > 90) rotateAngle = 90;
        else if (rotateAngle < -90) rotateAngle = -90;
        transform.Rotate(rotateAngle * Time.deltaTime * new Vector3(0, 0, 1));
    }

    private void SpeedCorrection()
    {
        float maxSpeed = gameObject.GetComponent<ShipStatus>().maxSpeed;
        if (maxSpeed == 0 && currentSpeed > 0 || moveTouchId < 0 && currentSpeed > 0)
        {
            currentSpeed -= 0.01f;
            gameObject.GetComponent<SailsController>().isMoving = false;
        }
        else if (moveTouchId >= 0 && currentSpeed < maxSpeed)
        {
            currentSpeed += 0.01f;
            gameObject.GetComponent<SailsController>().isMoving = true;
        }
        if (currentSpeed > 0 && currentSpeed < 0.0001f) currentSpeed = 0;

    }

    private void MovePlayer()
    {
        transform.Translate(currentSpeed * Time.deltaTime * new Vector3(0, 1, 0));
    }
}
