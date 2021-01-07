using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public Sprite icon;
    public float cooldown;
    public float cooldownTimer;
    public bool active;

    protected virtual void Start()
    {
        active = false;
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    public virtual void Activate()
    {
        if (!active && cooldownTimer <= 0)
        {
            Effect();
        }
    }

    protected virtual void Effect() {
        active = true;
    }

    protected virtual void Deactivate() {
        active = false;
        cooldownTimer = cooldown;

    }
}
