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
    override protected void OnStart() { }
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

    protected void GiveXp(int xpReceived)
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("player");
        foreach (GameObject playerObj in playerObjs)
        {
            if (playerObj.GetComponent<PlayerMovement>().currentState != PlayerState.disabled)
            {
                Player player = playerObj.GetComponent<Player>();
                player.ReceiveXp(xpReceived);
                if (player.rhino != null)
                    player.rhino.ReceiveXp(xpReceived);

            }
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
