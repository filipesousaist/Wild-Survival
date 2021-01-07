using UnityEngine;

public abstract class SimpleBuilding : Building
{
    private SpriteRenderer spriteRenderer;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
   
    protected override void OnHide()
    {
        spriteRenderer.color = Colors.TRANSPARENT;
    }

    protected override void OnShow()
    {
        spriteRenderer.color = Colors.OPAQUE;
    }
}
