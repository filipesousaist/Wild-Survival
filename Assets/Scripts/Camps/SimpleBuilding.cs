using UnityEngine;

public abstract class SimpleBuilding : Building
{
    protected SpriteRenderer spriteRenderer;
    
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

    protected void SetAlpha(float a)
    {
        Color temp = spriteRenderer.color;
        temp.a = a;
        spriteRenderer.color = temp;
    }
}
