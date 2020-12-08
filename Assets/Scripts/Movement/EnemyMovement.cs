using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public float maxAttackWaitTime;
    public float chaseRadius;
    public float attackRadius;
    public float attackDuration;

    private Transform target;
    private float attackWaitTime;

    // Start is called before the first frame update
    override protected void OnStart()
    {
        target = GameObject.FindWithTag("player").transform;
        attackWaitTime = maxAttackWaitTime;

        currentState = EnemyState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
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
                MoveCharacter(difference);
                ChangeState(EnemyState.walk);
            }
            else
            {
                difference = Vector3.zero;
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

        animator.SetBool("attacking", true);
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
