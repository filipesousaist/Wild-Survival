using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    walk,
    attack,
    combat,
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

    private bool onTrigger;

    private readonly object lock_ = new object();


    // Start is called before the first frame update
    override protected void OnStart()
    {
        currentState = PlayerState.walk;
        players = transform.parent.GetComponentsInChildren<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        onTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputEnabled) {
            if (currentState == PlayerState.combat)
            {
                ChangeState(PlayerState.walk);
            }
            agent.enabled = false;
            Vector3 difference = Vector3.zero;
            difference.x = Input.GetAxisRaw("Horizontal");
            difference.y = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
            {
                StartCoroutine(AttackCo());
            }
            else if (currentState == PlayerState.walk)
            {
                if (difference.magnitude > 0)
                {
                    MoveCharacter(difference);
                }
                else
                {
                    animator.SetBool("moving", false);
                }

                UpdateAnimation(difference);
            }
        }
        else if(currentState != PlayerState.dead)
        {
            agent.enabled = true;
            CombatUpdate();
            if (currentState == PlayerState.walk)
            {
                foreach (PlayerMovement player in players)
                {
                    if (player.inputEnabled)
                    {
                        playerToFollow = player;
                        break;
                    }
                }
                Vector3 difference = playerToFollow.transform.position - transform.position;
                if (currentState == PlayerState.walk)
                {
                    if (difference.magnitude < 3)
                    {
                        difference = Vector3.zero;
                        agent.velocity = Vector3.zero;
                        agent.isStopped = true;
                        animator.SetBool("moving", false);
                    }
                    else
                    {
                        animator.SetBool("moving", true);
                        agent.destination = playerToFollow.transform.position;
                        agent.isStopped = false;
                        difference = agent.velocity;
                        UpdateAnimation(difference);
                    }
                    UpdateAnimation(difference);

                }
            }
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
                agent.isStopped = false;
                animator.SetBool("moving", true);
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

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FadeObstacle") && other.isTrigger && Input.GetAxisRaw("Vertical") > 0 && !onTrigger)
        {
            onTrigger = true;
            var fadeScript = Canvas.FindObjectOfType<Fade>();

            yield return StartCoroutine(fadeScript.FadeToBlack());

            // Restaurar vida
            FindObjectOfType<ActivistsManager>().HealAll();
            FindObjectOfType<RhinosManager>().HealAll();

            animator.SetFloat("moveY", -1);

            yield return StartCoroutine(fadeScript.FadeToClear());
        }     
    }

    private void OnCollisionExit2D()
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

    public void Revive()
    {
        ChangeState(PlayerState.walk);
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
