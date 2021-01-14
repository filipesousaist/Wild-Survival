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
    private NavMeshAgent agent;

    override protected void OnAwake()
    {
        player = GetComponent<Rhino>().owner;
        if (player != null)
        {
            playerMov = player.GetComponent<PlayerMovement>();
        }
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        currentState = RhinoState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        commandDestination = Vector3.zero;
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // FixedUpdate is called periodically, with a fixed period
    void Update()
    {
        if (player != null)
        {
            if (!(currentState == RhinoState.flee || currentState == RhinoState.disabled) && player.rhino.health <= 0)
            {
                Flee();
            }
            if (playerMov.inputEnabled) {
                if (!(currentState == RhinoState.flee || currentState == RhinoState.disabled) &&
                    Input.GetMouseButtonDown(1))
                {
                    //evita controlar o rino quando se esta a mexer no inventario
                    if (!EventSystem.current.IsPointerOverGameObject()) { 
                        currentState = RhinoState.command;
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
        

    }

    public void SetPlayer(Player player, PlayerMovement playerMovement)
    {
        this.playerMov = playerMovement;
        this.player = player;
    }

    void CommandUpdate()
    {
        bool isNearDestination = (commandDestination - transform.position).magnitude <= destinationRadius;
        agent.isStopped = isNearDestination;
        animator.SetBool("moving", !isNearDestination);

        if (isNearDestination)
            currentState = RhinoState.combat;
        else
        {
            agent.destination = commandDestination;
            UpdateAnimation(agent.velocity);
        }
    }

    void CombatUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] dummies = GameObject.FindGameObjectsWithTag("dummy");
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
            if (closestEnemyDistance == 0 && dummies != null && playerMov.inputEnabled)
            {
                foreach (GameObject dummy in dummies)
                {
                    Vector3 dummyPosition = dummy.transform.position;
                    float distanceFromDummy = (dummyPosition - transform.position).magnitude;
                    if (distanceFromDummy < chaseRadius && closestEnemyDistance < distanceFromDummy)
                    {
                        target = dummy.transform;
                        closestEnemyDistance = distanceFromDummy;
                    }
                }
                /*Vector3 enemyPosition = dummy.transform.position;
                float distanceFromEnemy = (enemyPosition - transform.position).magnitude;
                if (distanceFromEnemy < chaseRadius)
                {
                    target = dummy.transform;
                    closestEnemyDistance = 1;
                }*/
            }
            if (closestEnemyDistance == 0)
                currentState = RhinoState.walk;
            else
            {
                InCombat();
                currentState = RhinoState.combat;
            }
            }
        else
            currentState = RhinoState.walk;
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
            Vector3 orientation = new Vector3(playerMov.animator.GetFloat("moveX"), playerMov.animator.GetFloat("moveY"));
            Vector3 destination = player.transform.position - orientation.normalized * 2;
            Vector3 direction = destination - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 4, LayerMask.GetMask("unwalkable"));
            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, destination, Color.red);
                destination = player.transform.position;
            }
            difference = agent.velocity;
            agent.destination = destination;
            agent.isStopped = false;
            animator.SetBool("moving", true);
            currentState = RhinoState.walk;
            DebugDrawPath(agent.path.corners);
            //Debug.Log(agent.velocity);
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
            currentState = RhinoState.disabled;
            agent.isStopped = true;
            agent.destination = transform.position;
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

    public override bool CanBeTargeted()
    {
        return !(currentState is RhinoState.disabled ||
                 currentState is RhinoState.flee);
    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        agent.isStopped = true;
        currentState = RhinoState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.62f);
        currentState = RhinoState.combat;
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
                animator.SetBool("moving", true);
                agent.isStopped = false;
                difference = agent.velocity;
                agent.destination = target.position;
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
        StopCoroutine("AttackCo");
        currentState = RhinoState.flee;
        agent.destination = escapeCoordinates;
        animator.SetBool("moving", true);
        agent.isStopped = false;
    }
}
