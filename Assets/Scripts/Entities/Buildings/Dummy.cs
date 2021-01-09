using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : SimpleBuilding
{
    public new BoxCollider2D collider2D;

    private float deadTime;

    private readonly float disableTime = 5;

    private float waitTime;

    private readonly float respawnTime = 5;

    private bool active = true;
    private bool shouldUpdate = true;

    override protected void OnStart() {
        base.OnStart();
        healthBar.transform.localPosition += new Vector3(0, -1.3f);
    }

    override protected void OnDeath()
    {
        animator.SetBool("Dead", true);
        collider2D.enabled = false;
        healthBar.transform.localScale = Vector3.zero;

        gameObject.tag = "deadDummy";
        deadTime = Time.time;
    }

    protected override void OnHide()
    {
        base.OnHide();
        shouldUpdate = false;
    }

    protected override void OnShow()
    {
        base.OnShow();
        shouldUpdate = true;
    }

    protected override void OnUpgrade()
    {
        //TODO
    }

    public override void Knock(float knockTime, float damage)
    {
        if (shouldUpdate)
            TakeDamage(damage);
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (shouldUpdate)
        {
            if (active && gameObject.CompareTag("deadDummy") && Time.time - deadTime > disableTime)
                Disable();

            if (!active && Time.time - waitTime > respawnTime)
                Respawn();
        }
    }

    protected override void ShowHealthBar(bool show)
    {
        healthBar.transform.localScale = show ? Vec3.HALF : Vector3.zero;
    }

    private void Disable()
    {
        active = false;
        SetAlpha(0);
        waitTime = Time.time;
    }

    private void Respawn()
    {
        active = true;
        animator.SetBool("Dead", false);
        collider2D.enabled = true;
        SetAlpha(1);
        gameObject.tag = "dummy";
        health = maxHealth;
        healthBar.transform.localScale = Vector3.one;
    }

    public float GetHealthFraction()
    {
        return health / maxHealth;
    }
}
