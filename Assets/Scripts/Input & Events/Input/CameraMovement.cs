using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed;
    private float zDistance;

    public bool cameraFollow { get; private set; } = false;
    Transform target; //Reference to camera taget object

    private void Start()
    {
        zDistance = transform.position.z;
    }
    void Update()
    {
        WSADMovement();

        if (cameraFollow) 
        {
            Follow(target);
        }
    }

    private void WSADMovement()
    {
        float x = Input.GetAxis("Horizontal"); //Unity has WSAD mapped already
        float y = Input.GetAxis("Vertical");

        Vector3 moveVec = new(x, y, 0);
        transform.Translate(moveVec * movementSpeed * Time.deltaTime);
    }
    public void PointTo(Vector3 position)
    {
        transform.position = new(position.x, position.y, zDistance);
    }
    public void StartFollow(Transform toTarget)
    {
        cameraFollow = true;
        target = toTarget;
    }
    public void Follow(Transform toTarget)
    {
        Vector3 pos = toTarget.transform.position;
        PointTo(pos);
    }
    public void StopFollow()
    {
        cameraFollow = false;
        target = null;
    }
}
