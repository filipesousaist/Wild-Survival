using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    walk,
    attack,
    stagger
}
public class EnemyMovement : EntityMovement
{
    [ReadOnly] public EnemyState currentState;
    [ReadOnly] public bool isVisible;
    [ReadOnly] public IEnemyTarget target;

    private NavMeshAgent agent;
    private Renderer myRenderer;
    private EnemyTargetAI targetAI;
    private EnemiesManager enemiesManager;

    public static readonly float UPDATE_TARGET_PERIOD = 0.1f;
    [ReadOnly] public float updateTargetTime;

    override protected void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        myRenderer = GetComponent<Renderer>();
        enemiesManager = FindObjectOfType<EnemiesManager>();
    }

    override protected void OnStart()
    {
        targetAI = enemiesManager.GetTargetAI();

        attackWaitTime = maxAttackWaitTime;
        updateTargetTime = UPDATE_TARGET_PERIOD;

        isVisible = false;
        currentState = EnemyState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void FixedUpdate()
    {
        Vector3 difference = targetAI.Update(this, Time.deltaTime);

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (target == null)
            Stop();
        else if (currentState != EnemyState.stagger && difference.magnitude <= attackRadius)
        {
            animator.SetBool("moving", false);
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo(difference));
                
        }
        else if (currentState == EnemyState.walk)
        {  
            if (enemiesManager.IgnoreChaseRadius() || difference.magnitude <= chaseRadius)
            {
                animator.SetBool("moving", true);
                agent.destination = target.GetPosition();
                difference = agent.velocity;
                agent.isStopped = false;
                currentState = EnemyState.walk;
            }
            else
            {
                difference = Vector3.zero;
                Stop();
            }
        }
        UpdateAnimation(difference);

        isVisible = myRenderer.isVisible;
    }

    private void Stop()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        animator.SetBool("moving", false);
    }

    private IEnumerator AttackCo(Vector3 difference)
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        agent.isStopped = true;
        currentState = EnemyState.attack;
        yield return null;
        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.34f);
        string correctHitbox = EnableCorrectHitbox(difference);
        yield return new WaitForSeconds(.32f);
        DisableHitbox(correctHitbox);
        yield return new WaitForSeconds(.023f);

        currentState = EnemyState.walk;
        attackedRecently = false;
    }

    override public IEnumerator KnockCo(float knockTime)
    {
        currentState = EnemyState.stagger;
        yield return base.KnockCo(knockTime);
        currentState = EnemyState.walk;
    }
}
