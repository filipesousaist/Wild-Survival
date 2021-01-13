using UnityEngine;

public class Dummy : SimpleBuilding
{
    public new BoxCollider2D collider2D;

    private float deadTime;

    private readonly float disableTime = 3;

    private float waitTime;

    private readonly float respawnTime = 3;

    private bool active = true;
    private bool shouldUpdate = true;

    private ActivistsManager activistsManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        activistsManager = FindObjectOfType<ActivistsManager>();
    }

    override protected void OnStart() {
        base.OnStart();
        healthBar.transform.localPosition += new Vector3(0, -1.3f);
    }

    override protected void OnDeath()
    {
        animator.SetBool("Dead", true);
        collider2D.enabled = false;
        ShowHealthBar(false);
        GiveTrainingXp(1);

        gameObject.tag = "deadDummy";
        deadTime = Time.time;
    }

    private void GiveTrainingXp(int trainingXp)
    {
        activistsManager.GetCurrentPlayer().rhino.ReceiveTrainingXp(trainingXp);
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
        ShowHealthBar(true);
    }

    public float GetHealthFraction()
    {
        return health / maxHealth;
    }
}
