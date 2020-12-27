﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    public Animator animator;
    protected Rigidbody2D myRigidBody;
    protected Vector3 change;
    private Entity entity;
    private KinematicBoxCollider2D kCollider;
    protected float attackWaitTime;


    public bool attackedRecently;
    public float speed;
    public float maxAttackWaitTime;
    public float chaseRadius;  // maximum distance to follow a zombie
    public float attackRadius;
    public float attackDuration;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        entity = GetComponent<Entity>();
        kCollider = GetComponent<KinematicBoxCollider2D>();
        attackedRecently = false;

        OnStart();
    }

    abstract protected void OnStart();

    public virtual IEnumerator KnockCo(Rigidbody2D myRigidBody, float knockTime)
    {
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
        }
    }

    public virtual void Flee()
    {

    }

    public virtual void Die()
    {

    }

    protected virtual void UpdateAnimation(Vector3 difference)
    {
        if (difference != Vector3.zero)
        {
            animator.SetFloat("moveX", difference.x);
            animator.SetFloat("moveY", difference.y);
        }
    }

    public void MoveCharacter(Vector3 difference)
    {
        animator.SetBool("moving", true);
       
        Vector3 offset = difference.normalized * Time.deltaTime * speed;
        if (kCollider.CanMove(offset))
            transform.position += offset;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (entity.IsOtherEntity(collision.collider.gameObject))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            myRigidBody.velocity = Vector2.zero;
        }
    }

    protected string EnableCorrectHitbox(Vector3 difference)
    {
        float y = difference.y;
        float x = difference.x;
        string correctHitbox;
        if (y > x)
        {
            if (y > -x)
                correctHitbox = "up hit box";
            else
                correctHitbox = "left hit box";
        }
        else
        {
            if (y > -x)
                correctHitbox = "right hit box";
            else
                correctHitbox = "down hit box";
        }

        string[] allHitboxes = { "right hit box", "up hit box", "left hit box", "down hit box" };
        foreach (string name in allHitboxes)
        {
            transform.Find(name).gameObject.SetActive(name == correctHitbox);
        }

        return correctHitbox;
    }

    protected void DisableHitbox(string name)
    {
        transform.Find(name).gameObject.SetActive(false);
    }
}
