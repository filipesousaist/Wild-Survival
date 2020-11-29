using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private Transform target;
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private float attackWaitTime;

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindWithTag("player").transform;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        attackWaitTime = maxAttackWaitTime;

        currentState = EnemyState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 difference = target.position - transform.position;

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (currentState != EnemyState.stagger && difference.magnitude <= attackRadius)
        {
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo());
        }
        else if (currentState == EnemyState.walk)
        {
            if (difference.magnitude <= chaseRadius)
            {
                MoveCharacter(difference);
            }
            else
            {
                difference = Vector3.zero;
                animator.SetBool("moving", false);
            }

            UpdateAnimation(difference);
        }
    }

    private void UpdateAnimation(Vector3 difference)
    {
        if (difference != Vector3.zero)
        {
            animator.SetFloat("moveX", difference.x);
            animator.SetFloat("moveY", difference.y);
        }

    }

    void MoveCharacter(Vector3 difference)
    {
        animator.SetBool("moving", true);
        myRigidBody.MovePosition(
            transform.position + difference.normalized * moveSpeed * Time.deltaTime
        );
        ChangeState(EnemyState.walk);
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        currentState = EnemyState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.68f);
        currentState = EnemyState.walk;
    }
}
