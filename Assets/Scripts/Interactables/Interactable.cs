using System.Collections;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool once = false; // Set to true in subclass if necessary
    private bool hasInteracted = false;

    public bool isInteractable = false;
    protected bool interacting = false;

    protected ActivistsManager activistsManager;
    private Collider2D trigger;

    private InteractText interactText;

    private void Awake()
    {
        trigger = InitTrigger();
        activistsManager = FindObjectOfType<ActivistsManager>();
        interactText = FindObjectOfType<InteractText>();
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

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() { }

    void Update()
    {
        if (IsPlayerTryingToInteract() && !(once && hasInteracted))
            StartCoroutine(InteractCo());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        TestPlayerNear(other);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        TestPlayerFar(other);
    }

    public void TestPlayerNear(Collider2D collider)
    {
        if (IsPlayerNear(collider, true))
        {
            isInteractable = true;
            interactText.SetInteractable(this);
            OnPlayerApproach();
        }
    }

    protected virtual void OnPlayerApproach() { }

    public void TestPlayerFar(Collider2D collider)
    {
        if (IsPlayerNear(collider, false))
        {
            isInteractable = false;
            OnPlayerMoveAway();
        }
    }

    protected virtual void OnPlayerMoveAway() { }

    public bool IsPlayerNear(Collider2D other, bool isNear)
    {
        if (other.CompareTag("player") && other.isTrigger &&
            (other.IsTouching(trigger) == isNear))
        {
            Player player = other.GetComponent<Player>();
            return player != null && activistsManager.IsCurrentActivist(player);
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
        AfterInteract();
    }

    protected virtual IEnumerator OnInteract()
    {
        Debug.Log("Interacting with " + transform.name);
        yield return null;
    }

    protected virtual void AfterInteract() {}

    public virtual string GetInteractText()
    {
        return null;
    }
}
