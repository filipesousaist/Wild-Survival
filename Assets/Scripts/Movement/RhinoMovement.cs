﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [ReadOnly] public RhinoState currentState;
    public float followRadius;  // maximum distance to follow an activist
    public float arriveRadius;
    public float maxAttackWaitTime;
    public float chaseRadius;  // maximum distance to follow a zombie
    public float attackRadius;
    public float attackDuration;
    public float destinationRadius;
    public Vector2 escapeCoordinates;

    private float attackWaitTime;
    private Transform target;
    private Player player;
    private PlayerMovement playerMov;
    private Vector3 commandDestination;
    private List<Vector2> path;
    private bool isFleeing;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMov.inputEnabled) { 
            if (!(currentState == RhinoState.flee || currentState == RhinoState.disabled) &&
                Input.GetMouseButtonDown(1))
            if (Input.GetMouseButtonDown(1))
            {
                ChangeState(RhinoState.command);
                Clicked();
            }
        }

        switch (currentState)
        {
            case RhinoState.command:
                CommandUpdate(); break;
            case RhinoState.combat:
                CombatUpdate(); break;
            case RhinoState.walk:
                WalkUpdate(); break;
            case RhinoState.flee:
                FleeUpdate(); break;
            case RhinoState.disabled:
                DisabledUpdate(); break;
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
        Vector3 difference = player.transform.position - transform.position;
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

    void FleeUpdate()
    {
        if (!isFleeing)
        {
            StartCoroutine(FollowPath());
            isFleeing = true;
        }

        
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

    IEnumerator FollowPath()
    {
        if (path.Count > 0)
        {
            var targetIndex = 0;
            Vector2 currentWaypoint = path[0];

            while (true)
            {
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Count)
                    {
                        animator.SetBool("moving", false);
                        ChangeState(RhinoState.disabled);
                        speed = 8;
                        isFleeing = false;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                Vector3 difference = new Vector3(currentWaypoint.x, currentWaypoint.y) - transform.position;
                UpdateAnimation(difference);
                yield return null;


            }
        }
    }

    public override void Flee()
    {
        ChangeState(RhinoState.flee);
        path = new List<Vector2>(Pathfinding.RequestPath(transform.position, escapeCoordinates));
        animator.SetBool("moving", true);
        speed = 5;
    }

    private void ChangeState(RhinoState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
