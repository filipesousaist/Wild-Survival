using System.Collections;
using System.Collections.Generic;
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

    private Transform target;
    private PlayerMovement playerToFollow;
    private NavMeshAgent agent;

    private Player player;
    private ActivistsManager activistsManager;
    private RhinosManager rhinosManager;

    private bool onTrigger;
    private bool inTent = false;

    private float walkOffset;

    // Start is called before the first frame update
    override protected void OnStart()
    {
        if (currentState != PlayerState.disabled)
        {
            currentState = PlayerState.walk;
        }
        player = GetComponent<Player>();
        players = transform.parent.GetComponentsInChildren<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        activistsManager = FindObjectOfType<ActivistsManager>();
        rhinosManager = FindObjectOfType<RhinosManager>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        onTrigger = false;
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
            ChangeState(PlayerState.walk);

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

        if (onTrigger && !inTent && difference.y > 0)
            StartCoroutine(RestInTent());
    }

    // Called when this player is being controlled by the NavMesh agent
    void AgentUpdate()
    {
        agent.enabled = true;
        CombatUpdate();
        if (currentState == PlayerState.walk)
        {
            playerToFollow = activistsManager.GetCurrentPlayerMovement();

            bool isNearTarget = (playerToFollow.transform.position - transform.position).magnitude < 3;
            agent.isStopped = isNearTarget;
            animator.SetBool("moving", !isNearTarget);

            Vector3 orientation = activistsManager.GetCurrentPlayerOrientation();
            Vector2 offset = KinematicBoxCollider2D.Rotate(orientation, walkOffset);
            Vector3 destination = playerToFollow.transform.position + new Vector3(offset.x, offset.y).normalized * 2;
            Vector3 direction = destination - transform.position;

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
                ChangeState(PlayerState.combat);
                InCombat();
            }
            else
            {
                ChangeState(PlayerState.walk);
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
                    ChangeState(PlayerState.walk);
                else
                {
                    ChangeState(PlayerState.combat);
                    InCombat();
                }
            }
            else
                ChangeState(PlayerState.walk);
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

    private IEnumerator RestInTent()
    {
        Fade fadeScript = FindObjectOfType<Fade>();

        inTent = true;
        inputEnabled = false;

        yield return StartCoroutine(fadeScript.FadeToBlack());

        // Restaurar vida
        activistsManager.HealAll();
        FindObjectOfType<RhinosManager>().HealAll();

        animator.SetFloat("moveY", -1);

        yield return StartCoroutine(fadeScript.FadeToClear());

        inTent = false;
        inputEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FadeObstacle") && other.isTrigger &&
            activistsManager.IsCurrentActivist(player))
        {
            onTrigger = true;
        }
    }

    private void OnTriggerExit2D()
    {
        onTrigger = false;
    }

    private IEnumerator AttackCo()
    {
        attackWaitTime = 0;

        animator.SetBool("moving", false);
        animator.SetBool("attacking",true);
        if (!inputEnabled)
            agent.isStopped = true;
        ChangeState(PlayerState.attack);
        yield return null;
        animator.SetBool("attacking",false);
        yield return new WaitForSeconds(.33f);
        if (inputEnabled)
            ChangeState(PlayerState.walk);
        else
            ChangeState(PlayerState.combat);
        attackedRecently = false;
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

    public void Revive()
    {
        if (currentState != PlayerState.dead)
        {
            ChangeState(PlayerState.walk);
        }
    }
    public override void Die()
    {
        ChangeState(PlayerState.dead);
        inputEnabled = false;
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
