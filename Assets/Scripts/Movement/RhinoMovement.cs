using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RhinoState
{
    walk,
    command,
    combat,
    attack
}
public class RhinoMovement : MonoBehaviour
{
    public RhinoState currentState;
    public float speed;
    public float followRadius;  // maximum distance to follow an activist
    public float arriveRadius;
    public float maxAttackWaitTime;
    public float chaseRadius;  // maximum distance to follow a zombie
    public float attackRadius;
    public float attackDuration;
    public float destinationRadius;
    public float lastAttackTime;

    private float attackWaitTime;
    private Transform target;
    private Transform player;
    private Rigidbody2D myRigidBody;
    private Vector3 commandDestination;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player").transform;
        currentState = RhinoState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        commandDestination = Vector3.zero;
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            ChangeState(RhinoState.command);
            Clicked();
        }
        
        if (currentState == RhinoState.command)
        {
            Vector3 difference = commandDestination - transform.position;
            attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);
            if (difference.magnitude > destinationRadius)
            {
                MoveCharacter(difference);
                UpdateAnimation(difference);
            }
            else
            {
                ChangeState(RhinoState.combat);
            }
        }

        else if (currentState == RhinoState.combat)
        {
            var enemies = GameObject.FindGameObjectsWithTag("enemy");
            if (enemies != null)
            {
                float closestEnemyDistance = 0f;
                foreach (var enemy in enemies)
                {
                    var enemyPosition = enemy.transform.position;
                    var distanceFromEnemy = (enemyPosition - transform.position).magnitude;
                    if (distanceFromEnemy < chaseRadius && closestEnemyDistance < distanceFromEnemy)
                    {
                        target = enemy.transform;
                        closestEnemyDistance = distanceFromEnemy;
                    }
                }
                if (closestEnemyDistance == 0)
                {
                    ChangeState(RhinoState.walk);
                }
                else
                {
                    InCombat();
                }

            }
            else
            {
                ChangeState(RhinoState.walk);
            }
        }

        else if (currentState == RhinoState.walk)
        {
            Vector3 difference = player.position - transform.position;
            if (difference.magnitude <= followRadius)
            {
                if (difference.magnitude > arriveRadius)
                {
                    MoveCharacter(difference);
                    ChangeState(RhinoState.walk);
                }
                else
                {
                    difference = Vector3.zero;
                    animator.SetBool("moving", false);
                }
            }
            else
            {
                difference = Vector3.zero;
                animator.SetBool("moving", false);
            }

            UpdateAnimation(difference);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player" || collision.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
        }

    }

    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        commandDestination.x = ray.origin.x;
        commandDestination.y = ray.origin.y;
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = RhinoState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.62f);
        currentState = RhinoState.combat;
    }

    void UpdateAnimation(Vector3 difference)
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
                MoveCharacter(difference);
            }
            else
            {
                difference = Vector3.zero;
                animator.SetBool("moving", false);
            }
        }
        UpdateAnimation(difference);

    }

    private void ChangeState(RhinoState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
