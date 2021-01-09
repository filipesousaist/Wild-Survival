using UnityEngine;

public abstract class Character : Entity, IEnemyTarget
{
    public Sprite portrait;

    public Signal healthSignal;
    public FloatValue barHealth;

    public Signal XpSignal;
    public IntValue barXp;

    [ReadOnly] public int xp;
    [ReadOnly] public int requiredXp;
    [ReadOnly] public int level;

    override protected void OnAwake() { }

    protected override void OnStart()
    {
        xp = 0;
        level = 1;
        UpdateRequiredXp();
    }

    override public void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        UpdateBarHealth();
    }

    public virtual void ReceiveXp(int xpReward)
    {
        xp += xpReward;
        while (xp >= requiredXp)
            LevelUp();
    }

    // Currently only damage increases on level up (see IncreaseAttributes in Player and Rhino)
    private void LevelUp()
    {
        xp -= requiredXp;
        level ++;

        UpdateRequiredXp();
        IncreaseAttributes();
    }

    protected abstract void UpdateRequiredXp();
    protected abstract void IncreaseAttributes();

    public abstract void UpdateBarHealth();

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetHealthFraction()
    {
        return health / maxHealth;
    }
}
