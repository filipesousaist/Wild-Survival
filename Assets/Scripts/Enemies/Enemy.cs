using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour
{
    private Animator animator;

    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public GameObject deathEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathEffect();
            this.gameObject.SetActive(false);
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

    public void Knock(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        StartCoroutine(knockCo(myRigidBody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator knockCo(Rigidbody2D myRigidBody, float knockTime)
    {
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
