using System.Collections.Generic;
using UnityEngine;

public class KinematicBoxCollider2D : MonoBehaviour
{
    private static readonly Vector2 E_X = new Vector2(1, 0);

    private Vector2 position;
    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;


    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        InitCorners();
    }

    void InitCorners()
    {
        BoxCollider2D environmentCollider = GetComponent<Entity>().GetEnvironmentCollider();

        position = environmentCollider.offset;
        Vector2 halfSize = environmentCollider.size / 2;
        float x1 = (position.x - halfSize.x) * transform.localScale.x;
        float x2 = (position.x + halfSize.x) * transform.localScale.x;
        float y1 = (position.y - halfSize.y) * transform.localScale.y;
        float y2 = (position.y + halfSize.y) * transform.localScale.y;
        bottomLeft = new Vector2(x1, y1);
        bottomRight = new Vector2(x2, y1);
        topLeft = new Vector2(x1, y2);
        topRight = new Vector2(x2, y2);   
    }

    public bool CanMove(Vector2 offset)
    {
        Vector2[] corners = { bottomLeft, bottomRight, topLeft, topRight };
        Vector2 center = myRigidbody.position + position * transform.localScale;
        center = new Vector2(Mathf.Floor(center.x) + .5f, Mathf.Floor(center.y) + .5f);

        //Debug.DrawLine(myRigidbody.position + bottomLeft, myRigidbody.position + bottomRight, Color.blue, 10);

        for (float angle = 0; angle < 2 * Mathf.PI; angle += Mathf.PI / 4)
        {
            Vector2 direction = Rotate(E_X, angle);
            Vector2 otherCenter = new Vector2(Mathf.Floor(center.x + direction.x) + .5f, Mathf.Floor(center.y + direction.y) + .5f);
            Vector2 difference = otherCenter - center;
            

            RaycastHit2D hit = Physics2D.Raycast(center, difference.normalized, difference.magnitude, LayerMask.GetMask("unwalkable"));        
            if (hit.collider != null)
            {
                /*
                Debug.Log(hit.collider.name);
                Debug.DrawLine(center + direction + topLeft, center + direction + topRight, Color.red);
                Debug.DrawLine(center + direction + topRight, center + direction + bottomRight, Color.red);
                Debug.DrawLine(center + direction + bottomRight, center + direction + bottomLeft, Color.red);
                Debug.DrawLine(center + direction + bottomLeft, center + direction + topLeft, Color.red);
                */
                foreach (Vector2 c in corners)
                    if (IsInSquare(otherCenter, myRigidbody.position + c + offset))
                        return false;
            }
        }
        return true;
    }

    public static Vector2 Rotate(Vector2 v, float angle)
    {
        return new Vector2(
        v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
        v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
        );
    }

    private static bool IsInSquare(Vector2 center, Vector2 point)
    {
        return point.x >= center.x - 0.5f && point.x < center.x + 0.5f &&
               point.y >= center.y - 0.5f && point.y < center.y + 0.5f;
        
    }
}
