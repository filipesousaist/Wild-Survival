using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public Sprite portrait;
    public Sprite selectPartySprite;

    public Signal healthSignal;
    public FloatValue barHealth;

    public Signal XpSignal;
    public IntValue barXp;
    [ReadOnly] public int xp;
    [ReadOnly] public int requiredXp;
    [ReadOnly] public int level;
    public Rhino rhino;


    override protected void OnAwake()
    {
        xp = 0;
        level = 1;
        requiredXp = level * 10;
    }
    override protected void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        UpdateBarHealth();
    }

    public override void FullRestore()
    {
        base.FullRestore();
        animator.SetBool("dead", false);
    }

    public void UpdateBarHealth()
    {
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();

        if (manager.IsCurrentActivist(this))
        {
            barHealth.value = Mathf.Max(health, 0);
            healthSignal.Raise();
        }
    }

    //for now just the level increases, don't know what else to do, perhaps damage and hp?
    public void ReceiveXp(int xpReward)
    {
        //for now a simple xp curve, can make more complex later
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        xp += xpReward;
        while (xp >= requiredXp)
            LevelUp();
        if (manager.IsCurrentActivist(this))
        {
            barXp.value = xp;
            XpSignal.Raise();
        }

        if (rhino != null)
        {
            rhino.ReceiveXp(xpReward);
        }
    }

    private void LevelUp()
    {
        xp -= requiredXp;
        level++;
        requiredXp = level * 10;

        baseAttack += 1;
    }


    override protected void OnDeath()
    {
        if (((PlayerMovement)movement).currentState != PlayerState.dead)
        {
            animator.SetBool("dead", true);
            movement.Die();
            StartCoroutine(DeathCo());
        }
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        manager.activistsDead++;

        if (manager.IsCurrentActivist(this))
        {
            manager.ChangePlayer();
        }
    }

    IEnumerator DeathCo()
    {
        animator.SetBool("firstTimeDying", true);
        yield return null;
        animator.SetBool("firstTimeDying", false);
        
    }
}
