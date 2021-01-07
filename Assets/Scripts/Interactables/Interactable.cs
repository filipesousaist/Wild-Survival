using System.Collections;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool once = false; // Set to true in subclass if necessary
    private bool hasInteracted = false;

    protected bool isInteractable = false;
    protected bool interacting = false;

    protected ActivistsManager activistsManager;
    private Collider2D trigger;

    private void Awake()
    {
        trigger = InitTrigger();
        activistsManager = FindObjectOfType<ActivistsManager>();
        OnAwake();
    }

    protected virtual void OnAwake() { }

    private Collider2D InitTrigger()
    {
        foreach (Collider2D col in GetComponents<Collider2D>())
            if (col.isTrigger)
                return col;
        return null;
    }

    void Update()
    {
        if (IsPlayerTryingToInteract() && !(once && hasInteracted))
            StartCoroutine(InteractCo());
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

    protected virtual bool IsPlayerTryingToInteract()
    {
        return isInteractable && Input.GetKeyDown(KeyCode.E) && !interacting;
    }

    private IEnumerator InteractCo()
    {
        interacting = hasInteracted = true;
        yield return OnInteract();
        interacting = false;
    }

    protected virtual IEnumerator OnInteract()
    {
        Debug.Log("Interacting with " + transform.name);
        yield return null;
    }
}
