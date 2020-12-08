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
public class RhinoMovement : EntityMovement
{
    [ReadOnly] public RhinoState currentState;
    public float followRadius;  // maximum distance to follow an activist
    public float arriveRadius;
    public float maxAttackWaitTime;
    public float chaseRadius;  // maximum distance to follow a zombie
    public float attackRadius;
    public float attackDuration;
    public float destinationRadius;

    private float attackWaitTime;
    private Transform target;
    private Transform player;
    private Vector3 commandDestination;

    // Start is called before the first frame update
    override protected void OnStart()
    {
        player = GameObject.FindWithTag("player").transform;
        currentState = RhinoState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        commandDestination = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeState(RhinoState.command);
            Clicked();
        }

        switch (currentState)
        {
            case RhinoState.command:
                CommandUpdate(); break;
            case RhinoState.combat:
                CombatUpdate(); break;
            case RhinoState.walk:
                WalkUpdate(); break;
        }
    }

    void CommandUpdate()
    {
        Vector3 difference = commandDestination - transform.position;
        attackWaitTime = System.Math.Min(attackWaitTime + Time.deltaTime, maxAttackWaitTime);
        if (difference.magnitude > destinationRadius)
        {
            MoveCharacter(difference);
            UpdateAnimation(difference);
        }
        else
            ChangeState(RhinoState.combat);
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
                InCombat();
        }
        else
            ChangeState(RhinoState.walk);
    }

    void WalkUpdate()
    {
        Vector3 difference = player.position - transform.position;
        if (difference.magnitude <= followRadius && difference.magnitude > arriveRadius)
        {
            MoveCharacter(difference);
            ChangeState(RhinoState.walk);
        }
        else
        {
            difference = Vector3.zero;
            animator.SetBool("moving", false);
        }

        UpdateAnimation(difference);
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
