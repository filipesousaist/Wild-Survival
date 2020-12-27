using System.Collections;
using UnityEngine;

public class Player : Entity
{
    private static readonly Color OPAQUE = new Color(1, 1, 1);
    private static readonly Color TRANSPARENT = new Color(1, 1, 1, 0.5f);

    public Signal healthSignal;
    public Signal XpSignal;
    public FloatValue barHealth;
    public IntValue barXp;
    public int level;
    public int xp;
    public int requiredXp;


    override protected void OnAwake()
    {
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
        {
            xp -= requiredXp;
            level++;
            requiredXp = level * 10;
        }
        if (manager.IsCurrentActivist(this))
        {
            barXp.value = xp;
            XpSignal.Raise();
        }
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

        if (manager.IsCurrentActivist(this))
        {
            manager.activistDead++;
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
