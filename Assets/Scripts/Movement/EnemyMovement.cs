using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}
public class EnemyMovement : EntityMovement
{
    [ReadOnly] public EnemyState currentState;

    private Transform target;
    private NavMeshAgent agent;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    override protected void OnStart()
    {
        target = GameObject.FindWithTag("player").transform;
        attackWaitTime = maxAttackWaitTime;

        currentState = EnemyState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        lastPosition = transform.position;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 difference = UpdateTarget();

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (currentState != EnemyState.stagger && difference.magnitude <= attackRadius)
        {
            animator.SetBool("moving", false);
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo(difference));
                
        }
        else if (currentState == EnemyState.walk || 
            currentState == EnemyState.idle)
        {  
            if (difference.magnitude <= chaseRadius)
            {
                animator.SetBool("moving", true);
                agent.destination = target.transform.position;
                difference = agent.velocity;
                agent.isStopped = false;
                ChangeState(EnemyState.walk);
            }
            else
            {
                difference = Vector3.zero;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                animator.SetBool("moving", false);
            }
        }
        UpdateAnimation(difference);
    }

    private Vector3 UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        IEnumerable<GameObject> possibleTargets = players.Concat(rhinos);

        Vector3 difference = Vector3.zero;
        target = null;
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (target != null)
            {
                Transform newTarget = possibleTarget.transform;
                Vector3 newDifference = newTarget.position - transform.position;

                if (newDifference.magnitude < difference.magnitude)
                {
                    target = newTarget;
                    difference = newDifference;
                }
            }
            else
            {
                target = possibleTarget.transform;
                difference = target.position - transform.position;
            }
        }

        return difference;    
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private IEnumerator AttackCo(Vector3 difference)
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        agent.isStopped = true;
        ChangeState(EnemyState.attack);
        yield return null;
        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.34f);
        string correctHitbox = EnableCorrectHitbox(difference);
        yield return new WaitForSeconds(.32f);
        DisableHitbox(correctHitbox);
        yield return new WaitForSeconds(.023f);

        ChangeState(EnemyState.walk);
        attackedRecently = false;
    }

    override public IEnumerator KnockCo(Rigidbody2D myRigidBody, float knockTime)
    {
        currentState = EnemyState.stagger;
        yield return base.KnockCo(myRigidBody, knockTime);
        currentState = EnemyState.idle;
    }
}
