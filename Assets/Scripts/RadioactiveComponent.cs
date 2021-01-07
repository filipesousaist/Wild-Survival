using UnityEngine;

public class RadioactiveComponent : MonoBehaviour
{
    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("rhino"))
        {
            other.GetComponent<Rhino>().ReceiveRadiation(2);
        }

        else if (other.CompareTag("player"))
        {
            other.GetComponent<Player>().TakeDamage(2);
        }

        Destroy(gameObject);
    }
}
