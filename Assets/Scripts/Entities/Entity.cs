using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Animator animator;
    protected EntityMovement movement;

    public FloatValue maxHealth;
    public string entityName;
    public int baseAttack;
    [ReadOnly] public float health;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<EntityMovement>();
    }

    protected void Awake()
    {
        health = maxHealth.value;
        OnAwake();
    }
    abstract protected void OnAwake();

    protected virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            OnDeath();
    }

    protected abstract void OnDeath();

    public void Knock(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        StartCoroutine(movement.KnockCo(myRigidBody, knockTime));
        TakeDamage(damage);
    }

    public virtual void FullRestore()
    {
        health = maxHealth.value;
    }

    public bool IsOtherEntity(GameObject gameObject)
    {
        return (gameObject.CompareTag("player") ||
                gameObject.CompareTag("rhino") ||
                gameObject.CompareTag("enemy")) &&
                !(gameObject == this.gameObject);
    }
}
