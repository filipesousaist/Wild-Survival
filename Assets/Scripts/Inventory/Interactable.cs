using UnityEngine;

public class Interactable : MonoBehaviour
{
    bool hasInteracted = false;

    bool isInteractable = false;

    private ActivistsManager activistsManager;
    private Collider2D trigger;

    private void Awake()
    {
        trigger = InitTrigger();
        activistsManager = FindObjectOfType<ActivistsManager>();
    }

    private Collider2D InitTrigger()
    {
        foreach (Collider2D col in GetComponents<Collider2D>())
            if (col.isTrigger)
                return col;
        return null;
    }

    public virtual void Interact() 
    {
        Debug.Log("Interacting with " + transform.name);
    }

    void Update()
    {
        if(isInteractable && Input.GetKeyDown(KeyCode.E)) {
            Interact();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerNear(other, true))
            isInteractable = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayerNear(other, false))
            isInteractable = false;
    }

    private bool IsPlayerNear(Collider2D other, bool isNear)
    {
        if (other.CompareTag("player") && other.isTrigger &&
            (other.IsTouching(trigger) == isNear))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && activistsManager.IsCurrentActivist(player))
                return true;
        }
        return false;
    }
}
