using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    walk,
    attack
}

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private float attackWaitTime;
    private EnemyState currentState;

    public float speed;
    public float chaseRadius;
    public float attackRadius;
    public float attackDuration;
    public float maxAttackWaitTime;

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
    void Update()
    {
        Vector3 difference = target.position - transform.position;

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (difference.magnitude <= attackRadius)
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

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetBool("moving", true);
        myRigidBody.MovePosition(
            transform.position + difference.normalized * speed * Time.deltaTime
        );
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
