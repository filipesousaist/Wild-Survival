using UnityEngine;

public class Player : Entity
{
    public Signal healthSignal;
    public FloatValue barHealth;

    override protected void OnAwake()
    {
        UpdateBarHealth();
    }
    override protected void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateBarHealth();
    }

    public void UpdateBarHealth()
    {
        barHealth.value = Mathf.Max(health, 0);
        healthSignal.Raise();
    }

    //TODO: improve method
    override protected void OnDeath()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }
}
