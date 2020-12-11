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
        GetComponent<SpriteRenderer>().color = OPAQUE;
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
        GetComponent<SpriteRenderer>().color = TRANSPARENT;
    }
}
