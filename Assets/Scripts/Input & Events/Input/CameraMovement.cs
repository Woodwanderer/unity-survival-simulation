using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera cam;
    public float movementSpeed;
    private float zDistance = -10;
    Vector3 velocity; //for SmoothPointTo()

    public bool cameraFollow { get; private set; } = false;
    Transform target; //Reference to camera taget object
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        WSADMovement();

        if (cameraFollow) 
        {
            Follow(target);
        }
    }
    private void LateUpdate()
    {
        CameraZoom();
    }
    private void WSADMovement()
    {
        Vector2 input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.sqrMagnitude < 0.001f) 
            return;

        StopFollow();

        Vector2 clamped = Vector2.ClampMagnitude(input, 1f); // prevents diagonal speed up
        Vector3 moveVec = new(clamped.x, clamped.y, 0);
        transform.Translate(moveVec * movementSpeed * Time.deltaTime);
    }
    void CameraZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0)
            return;

        cam.orthographicSize -= scroll * 0.25f;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5f, 20f);
    }
    public void PointTo(Vector3 position)
    {
        transform.position = new(position.x, position.y, zDistance);
    }
    public void SmoothPointTo(Vector3 target, float smoothTime = 1.5f)
    {
        Vector3 desired = new Vector3(target.x, target.y, zDistance);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref velocity,
            smoothTime
        );
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
