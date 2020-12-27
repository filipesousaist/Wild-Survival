using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

public enum RhinoState
{
    walk,
    command,
    combat,
    attack,
    flee,
    disabled
}
public class RhinoMovement : EntityMovement
{
    public RhinoState currentState;
    public float followRadius;  // maximum distance to follow an activist
    public float arriveRadius;
    public float destinationRadius;
    public Vector2 escapeCoordinates;

    private Transform target;
    private Player player;
    private PlayerMovement playerMov;
    private Vector3 commandDestination;
    private List<Vector2> path;
    private bool isFleeing;
    private NavMeshAgent agent;


    // Start is called before the first frame update
    override protected void OnStart()
    {
        player = GetComponent<Rhino>().owner;
        playerMov = player.GetComponent<PlayerMovement>();
        currentState = RhinoState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        commandDestination = Vector3.zero;
        isFleeing = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // FixedUpdate is called periodically, with a fixed period
    void Update()
    {
        if (playerMov.inputEnabled) {
            if (!(currentState == RhinoState.flee || currentState == RhinoState.disabled) &&
                Input.GetMouseButtonDown(1))
            {
                //evita controlar o rino quando se esta a mexer no inventario
                if (!EventSystem.current.IsPointerOverGameObject()) { 
                    ChangeState(RhinoState.command);
                    Clicked();
                }
            }
        }

        switch (currentState)
        {
            case RhinoState.command:
                CommandUpdate(); break;
            case RhinoState.combat:
                CombatUpdate(); break;
            case RhinoState.walk:
                WalkUpdate();
                if (!playerMov.inputEnabled)
                    CombatUpdate();
                break;
            case RhinoState.flee:
                FleeUpdate(); break;
            case RhinoState.disabled:
                DisabledUpdate(); break;
        }
    }

    void CommandUpdate()
    {
        bool isNearDestination = (commandDestination - transform.position).magnitude <= destinationRadius;
        agent.isStopped = isNearDestination;
        animator.SetBool("moving", !isNearDestination);

        if (isNearDestination)
            ChangeState(RhinoState.combat);
        else
        {
            agent.destination = commandDestination;
            UpdateAnimation(agent.velocity);
        }
    }

    void CombatUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies != null)
        {
            float closestEnemyDistance = 0f;
            foreach (GameObject enemy in enemies)
            {
                Vector3 enemyPosition = enemy.transform.position;
                float distanceFromEnemy = (enemyPosition - transform.position).magnitude;
                if (distanceFromEnemy < chaseRadius && closestEnemyDistance < distanceFromEnemy)
                {
                    target = enemy.transform;
                    closestEnemyDistance = distanceFromEnemy;
                }
            }
            if (closestEnemyDistance == 0)
                ChangeState(RhinoState.walk);
            else
            {
                InCombat();
                ChangeState(RhinoState.combat);
            }
            }
        else
            ChangeState(RhinoState.walk);
    }
    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        int i = 0;
        for (; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
        }
        Debug.DrawLine(corners[0], corners[1], Color.red);
    }

    void WalkUpdate()
    {
        Vector3 difference = player.transform.position - transform.position;
        if (difference.magnitude <= followRadius && difference.magnitude > arriveRadius)
        {
            //MoveCharacter(difference);
            agent.destination = player.transform.position;
            agent.isStopped = false;
            animator.SetBool("moving", true);
            difference = agent.velocity;
            ChangeState(RhinoState.walk);
            DebugDrawPath(agent.path.corners);
        }
        else
        {
            difference = Vector3.zero;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            animator.SetBool("moving", false);
        }

        UpdateAnimation(difference);
    }

    void FleeUpdate()
    {
        Vector3 difference = new Vector3(escapeCoordinates.x, escapeCoordinates.y) - transform.position;
        if (difference.magnitude < 1)
        {
            animator.SetBool("moving", false);
            ChangeState(RhinoState.disabled);
            agent.isStopped = true;
            agent.destination = transform.position;
            isFleeing = false;
            agent.velocity = Vector3.zero;
        }

        UpdateAnimation(agent.velocity);
        
    }

    void DisabledUpdate()
    {
        UpdateAnimation(Vector3.zero);
    }

    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        commandDestination.x = ray.origin.x;
        commandDestination.y = ray.origin.y;
    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        agent.isStopped = true;
        ChangeState(RhinoState.attack);
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.62f);
        ChangeState(RhinoState.combat);
        attackedRecently = false;
    }

    override protected void UpdateAnimation(Vector3 difference)
    {
        base.UpdateAnimation(difference);
        if (difference != Vector3.zero)
        {
            UpdateHurtbox(difference);
        }
    }

    private void UpdateHurtbox(Vector3 difference)
    {
        float newWidth;
        if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
            newWidth = 2.4f;
        else
            newWidth = 4f;
        
        List<BoxCollider2D> colliders = new List<BoxCollider2D>(GetComponents<BoxCollider2D>());
        BoxCollider2D hurtbox = colliders.Find((BoxCollider2D collider) => collider.isTrigger);
        hurtbox.size = new Vector2(newWidth, hurtbox.size.y);
    }

    void InCombat()
    {
        Vector3 difference = target.position - transform.position;

        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (difference.magnitude <= attackRadius)
        {
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo());
        }
        else
        {
            if (difference.magnitude <= chaseRadius)
            {
                agent.destination = target.position;
                difference = agent.velocity;
                //MoveCharacter(difference);
            }
            else
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                difference = Vector3.zero;
                animator.SetBool("moving", false);
            }
        }
        UpdateAnimation(difference);

    }

    public override void Flee()
    {
        ChangeState(RhinoState.flee);
        agent.destination = escapeCoordinates;
        animator.SetBool("moving", true);
        agent.isStopped = false;
        isFleeing = true;
    }

    private void ChangeState(RhinoState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
