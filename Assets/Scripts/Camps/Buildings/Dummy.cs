using UnityEngine;

public class Dummy : SimpleBuilding
{
    public new BoxCollider2D collider2D;

    public Vector2[] extraSpawnPoints;
    private Vector2 baseSpawnPoint;

    private float deadTime;

    private readonly float disableTime = 3;

    private float waitTime;

    private readonly float respawnTime = 3;

    private bool active = true;
    private bool shouldUpdate = true;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    override protected void OnStart() {
        base.OnStart();
        baseSpawnPoint = transform.localPosition;
        healthBar.transform.localPosition += new Vector3(0, -1.3f);
    }

    override protected void OnDeath(Entity killer)
    {
        if (killer.CompareTag("enemy"))
            base.OnDeath();
        else
        {
            animator.SetBool("Dead", true);
            collider2D.enabled = false;
            ShowHealthBar(false);
            if (killer.CompareTag("rhino"))
                GiveTrainingXp((Rhino) killer, 1);

            gameObject.tag = "deadDummy";
            deadTime = Time.time;
        }
        
    }

    private void GiveTrainingXp(Rhino rhino, int trainingXp)
    {
        rhino.ReceiveTrainingXp(trainingXp);
    }

    protected override void OnHide()
    {
        base.OnHide();
        shouldUpdate = false;
    }

    protected override void OnShow()
    {
        base.OnShow();
        transform.localPosition = baseSpawnPoint;
        gameObject.layer = Layers.DEFAULT;
        shouldUpdate = true;
    }

    protected override void OnUpgrade()
    {
        //TODO
    }

    public override void Knock(float knockTime, float damage, Collider2D attackerCol)
    {
        if (shouldUpdate)
            TakeDamage(damage, attackerCol.GetComponentInParent<Entity>());
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
        RandomSpawn();
        animator.SetBool("Dead", false);
        collider2D.enabled = true;
        SetAlpha(1);
        gameObject.tag = "dummy";
        health = maxHealth;
        ShowHealthBar(true);
    }

    private void RandomSpawn()
    {
        int r = Random.Range(0, extraSpawnPoints.Length + 1);
        transform.localPosition =
            r == 0 ? baseSpawnPoint : extraSpawnPoints[r - 1];
                                            
    }

    public float GetHealthFraction()
    {
        return health / maxHealth;
    }
}
