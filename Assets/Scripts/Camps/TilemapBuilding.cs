using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TilemapBuilding : Building
{
    private TilemapRenderer tilemapRenderer;

    protected override void OnAwake()
    {
        base.OnAwake();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }
    protected override void OnHide()
    {
        tilemapRenderer.enabled = false;
    }

    protected override void OnShow()
    {
        tilemapRenderer.enabled = true;
    }
}
