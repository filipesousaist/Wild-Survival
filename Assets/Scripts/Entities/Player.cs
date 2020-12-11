using System.Collections;
using UnityEngine;

public class Player : Entity
{
    private static readonly Color OPAQUE = new Color(1, 1, 1);
    private static readonly Color TRANSPARENT = new Color(1, 1, 1, 0.5f);

    public Signal healthSignal;
    public FloatValue barHealth;


    override protected void OnAwake()
    {
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

    //TODO: improve method
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
