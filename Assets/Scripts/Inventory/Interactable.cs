using UnityEngine;

public class Interactable : MonoBehaviour
{

    

    bool hasInteracted = false;

    bool isInteractable = false;
    public virtual void Interact() 
    {
        Debug.Log("Interacting with " + transform.name);
    }

    void Update()
    {
            //float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(/*distance <= radius*/ isInteractable && Input.GetKeyDown(KeyCode.E)) {
                Interact();
            }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isInteractable = true;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("player") )
        {
            isInteractable = false;
        }
    }
}
