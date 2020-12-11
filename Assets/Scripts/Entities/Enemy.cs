using UnityEngine;

public class Enemy : Entity
{
    public GameObject deathEffect;

    override protected void OnAwake() { }
    override protected void OnDeath()
    {
        DeathEffect();
        Destroy(gameObject);
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
}
