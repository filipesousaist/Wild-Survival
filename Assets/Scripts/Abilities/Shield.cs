using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Ability
{
    public GameObject shield;

    public float shieldTime;

    public bool active;

    SpriteRenderer[] sprites;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sprites = shield.GetComponentsInChildren<SpriteRenderer>();
        if (shieldTime > cooldown)
        {
            cooldown = shieldTime + 2;
        }
        active = false;
    }

    protected override void Update()
    {
        if (active && Time.time - lastUsed >= shieldTime) {
            Deactivate();
        }
    }

    protected override void Deactivate() {
        foreach (var item in sprites)
        {
            item.enabled = false;
        }
        active = false;
        lastUsed = Time.time;
    }

    protected override void Effect()
    {
        base.Effect();
        active = true;
        foreach (var item in sprites)
        {
            item.enabled = true;
        }
    }
}
