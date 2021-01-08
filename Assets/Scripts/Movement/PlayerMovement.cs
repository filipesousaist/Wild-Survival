using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum PlayerState
{
    walk,
    attack,
    combat,
    disabled,
    dead
}

public class PlayerMovement : EntityMovement
{
    public PlayerState currentState;
    public bool inputEnabled;
    public PlayerMovement[] players;

    private KinematicBoxCollider2D kCollider;

    private Transform target;
    private PlayerMovement playerToFollow;
    private NavMeshAgent agent;

    private Player player;
    private ActivistsManager activistsManager;

    private float walkOffset;

    override protected void OnAwake()
    {
        player = GetComponent<Player>();
        players = transform.parent.GetComponentsInChildren<PlayerMovement>();
        kCollider = GetComponent<KinematicBoxCollider2D>();
        agent = GetComponent<NavMeshAgent>();
        activistsManager = FindObjectOfType<ActivistsManager>();
    }

    override protected void OnStart()
    {
        if (currentState != PlayerState.disabled)
        {
            currentState = PlayerState.walk;
        }
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != PlayerState.disabled)
        {
            if (activistsManager.IsCurrentActivist(player))
                InputControlUpdate();
            else if (currentState != PlayerState.dead)
                AgentUpdate();
        }
        else
        {
            agent.velocity = Vector3.zero;
            animator.SetBool("moving", false);
            animator.SetBool("attacking", false);
        }
    }

    // Called when this player is being controlled by input (keyboard)
    void InputControlUpdate()
    {
        agent.enabled = false;

        if (currentState == PlayerState.combat)
            currentState = PlayerState.walk;

        if (inputEnabled)
        {
            Vector2 difference;
            difference.x = Input.GetAxisRaw("Horizontal");
            difference.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
                StartCoroutine(AttackCo());
            else if (currentState == PlayerState.walk)
            {
                if (difference.magnitude > 0)
                {
                    MoveCharacter(difference);
                }

                else
                    animator.SetBool("moving", false);

                difference.x = Mathf.Round(difference.x);
                difference.y = Mathf.Round(difference.y);
                UpdateAnimation(difference);
            }
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    // Called when this player is being controlled by the NavMesh agent
    void AgentUpdate()
    {
        agent.enabled = true;
        CombatUpdate();
        if (currentState == PlayerState.walk)
        {
            playerToFollow = activistsManager.GetCurrentPlayerMovement();


            Vector3 orientation = activistsManager.GetCurrentPlayerOrientation();
            Vector2 offset = KinematicBoxCollider2D.Rotate(orientation, walkOffset);
            Vector3 destination = playerToFollow.transform.position + new Vector3(offset.x, offset.y).normalized * 2;
            Vector3 direction = destination - transform.position;

            bool isNearTarget = (playerToFollow.transform.position - transform.position).magnitude < 2;
            agent.isStopped = isNearTarget;
            animator.SetBool("moving", !isNearTarget);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 4f, LayerMask.GetMask("unwalkable"));
            if (hit.collider != null)
            {
                destination = playerToFollow.transform.position;
            }
            agent.destination = destination;
            
            UpdateAnimation(agent.velocity);
        }
    }

    void CombatUpdate()
    {
        if (target != null)
        {
            float distanceFromEnemy = (target.position - transform.position).magnitude;
            if (distanceFromEnemy < chaseRadius )
            {
                currentState = PlayerState.combat;
                InCombat();
            }
            else
            {
                currentState = PlayerState.walk;
            }
        }
        else
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
                    currentState = PlayerState.walk;
                else
                {
                    currentState = PlayerState.combat;
                    InCombat();
                }
            }
            else
                currentState = PlayerState.walk;
        }
    }

    void InCombat()
    {
        Vector3 difference = target.position - transform.position;

        attackWaitTime = Mathf.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);

        if (difference.magnitude <= attackRadius)
        {
            if (attackWaitTime == maxAttackWaitTime)
                StartCoroutine(AttackCo());
        }
        else
        {
            bool targetInRange = difference.magnitude <= chaseRadius;
            agent.isStopped = !targetInRange;
            animator.SetBool("moving", targetInRange);

            if (difference.magnitude <= chaseRadius)
            {
                difference = agent.velocity;
                agent.destination = target.position;
            }
            else
            {
                difference = agent.velocity = Vector3.zero;
            }
        }
        UpdateAnimation(difference);
    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking",true);
        if (!inputEnabled)
            agent.isStopped = true;
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking",false);
        yield return new WaitForSeconds(.33f);
        currentState = inputEnabled ? PlayerState.walk : PlayerState.combat;
        attackedRecently = false;
    }

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetBool("moving", true);

        Vector3 offset = difference.normalized * Time.deltaTime * speed;
        if (kCollider.CanMove(offset))
            transform.position += offset;

    }

    public override bool CanBeTargeted()
    {
        return !(currentState is PlayerState.disabled || 
                 currentState is PlayerState.dead);
    }
    public void TeleportPlayer(Vector3 newPosition)
    {
        agent.Warp(newPosition);
    }

    public void TeleportRhino()
    {
        if (player.rhino != null)
        {
            player.rhino.transform.position = transform.position;
        }
    }

    public void EnableRhino()
    {
        if (player.rhino != null)
            player.rhino.transform.gameObject.SetActive(true);
    }

    public void DisableRhino()
    {
        if (player.rhino != null)
            player.rhino.transform.gameObject.SetActive(false);
    }

    public bool IsDead()
    {
        return player.health <= 0;
    }

    public Sprite GetSelectPartySprite()
    {
        if (player == null)
            player = GetComponent<Player>();
        return player.selectPartySprite;
    }

    public void SetWalkOffset(float offset) {
        walkOffset = Mathf.Deg2Rad * offset;
    }

    public void Revive(Vector3 newPosition)
    {
        if (currentState == PlayerState.dead)
        {
            currentState = PlayerState.walk;
            TeleportPlayer(newPosition);
            TeleportRhino();
        }
    }
    public override void Die()
    {
        currentState = PlayerState.dead;
        inputEnabled = false;
    }

}
