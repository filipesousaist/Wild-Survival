using UnityEngine;

public abstract class SimpleBuilding : Building
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D trigger;
    protected override void OnAwake()
    {
        base.OnAwake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trigger = InitTrigger();
    }

    private BoxCollider2D InitTrigger()
    {
        foreach (BoxCollider2D col in GetComponents<BoxCollider2D>())
            if (col.isTrigger)
                return col;
        return null;
    }
    protected override void OnHide()
    {
        spriteRenderer.color = Colors.TRANSPARENT;
        trigger.enabled = false;
    }

    protected override void OnShow()
    {
        spriteRenderer.color = Colors.OPAQUE;
        trigger.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerNear(other, true))
            playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayerNear(other, false))
            playerNear = false;
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
