using UnityEngine;

public class Enemy : Entity
{
    public Signal deathSignal;
    public GameObject deathEffect;
    public LootTable lootTable;
    [ReadOnly] public int wave;
    public int xpReward;
    private bool xpGiven = false;
    override protected void OnAwake() { }
    override protected void OnDeath()
    {
        DeathEffect();
        deathSignal.Raise();
         MakeLoot();
        Destroy(gameObject);
        if (!xpGiven)
        {
            GiveXp(xpReward);
            xpGiven = true;
        }
    }

    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Animator effectAnimator = effect.GetComponent<Animator>();
            effectAnimator.SetFloat("moveX", animator.GetFloat("moveX"));
            effectAnimator.SetFloat("moveY", animator.GetFloat("moveY"));
        }
    }

    private void MakeLoot() 
    {
        if (lootTable != null)
        {
            GameObject item = lootTable.LootItem();
            if (item != null)
            {
                Instantiate(item, transform.position, transform.rotation);
            }
        }
    }
}
