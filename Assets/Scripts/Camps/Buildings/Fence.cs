
using System.Collections;
using UnityEngine;

public class Fence : TilemapBuilding
{
    public Vector2 healthBarOffset;

    protected override void OnStart()
    {
        healthBar.transform.localPosition += (Vector3) healthBarOffset;
        base.OnStart();
    }

    protected override void OnUpgrade()
    {
        maxHealth += 50;
    }
}
