using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    private Transform target;
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private float attackWaitTime;
    private Enemy enemy;

    public float maxAttackWaitTime;
    public float chaseRadius;
    public float attackRadius;
    public float attackDuration;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("player").transform;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        attackWaitTime = maxAttackWaitTime;
        enemy = GetComponent<Enemy>();

        enemy.currentState = EnemyState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 difference = target.position - transform.position;

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (enemy.currentState != EnemyState.stagger && difference.magnitude <= attackRadius)
        {
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo());
        }
        else if (enemy.currentState == EnemyState.walk || 
            enemy.currentState == EnemyState.idle)
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
            transform.position + difference.normalized * speed * Time.deltaTime
        );
        ChangeState(EnemyState.walk);
    }

    private void ChangeState(EnemyState newState)
    {
        if (enemy.currentState != newState)
        {
            enemy.currentState = newState;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "rhino" || collision.gameObject.tag == "player")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            myRigidBody.velocity = Vector2.zero;
        }

    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        enemy.currentState = EnemyState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.68f);
        enemy.currentState = EnemyState.walk;
    }
}
