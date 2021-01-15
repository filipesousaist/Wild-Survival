using UnityEngine;

public static class Cam
{
    public static bool IsPointInView(Vector2 point)
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(point);

        return 0 <= viewportPoint.x && viewportPoint.x <= 1
            && 0 <= viewportPoint.y && viewportPoint.y <= 1;
    }
}
