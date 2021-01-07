using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Ability
{
    public GameObject shield;

    public float shieldTime;

    SpriteRenderer[] sprites;

    float shieldStart;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sprites = shield.GetComponentsInChildren<SpriteRenderer>();
        if (shieldTime > cooldown)
        {
            cooldown = shieldTime + 2;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (active && Time.time - shieldStart >= shieldTime) {
            Deactivate();
        }
    }

    protected override void Deactivate() {
        foreach (var item in sprites)
        {
            item.enabled = false;
        }
        base.Deactivate();
    }

    protected override void Effect()
    {
        base.Effect();
        shieldStart = Time.time;
        foreach (var item in sprites)
        {
            item.enabled = true;
        }
    }
}
