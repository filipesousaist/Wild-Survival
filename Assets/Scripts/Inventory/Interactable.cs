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
        if (!hasInteracted) {
            //float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(/*distance <= radius*/ isInteractable && Input.GetKeyDown(KeyCode.E)) {
                Interact();
                hasInteracted = true;
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("player") )
        {
            isInteractable = false;
        }
    }
}
