using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    public Vector2 bottomLeft;
    public Vector2 topRight;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetCamLimits();

        Camera cam = GetComponent<Camera>();
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = cam.transform.up;

        //Camera.main.orthographicSize *= 5;
    }

    void SetCamLimits()
    {
        float halfH = Camera.main.orthographicSize;
        float halfW = halfH * Camera.main.aspect;
        Vector2 offset = new Vector2(halfW, halfH);
        minPosition = bottomLeft + offset;
        maxPosition = topRight - offset;
    }

    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            ClampPosition(ref targetPosition);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    void ClampPosition(ref Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
        position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);
    }
}
