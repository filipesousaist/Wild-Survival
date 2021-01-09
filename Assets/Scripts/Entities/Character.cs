using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
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

    public override void Heal(float healValue)
    {
        base.Heal(healValue);
        UpdateBarHealth();
    }

    protected abstract void UpdateRequiredXp();
    protected abstract void IncreaseAttributes();

    public abstract void UpdateBarHealth();

}
