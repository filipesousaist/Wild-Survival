﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public GameObject player;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;

    private static readonly float MIN_DIST = 1;
    private static readonly float MAX_DIST = 12;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = player.transform.position.x - transform.position.x;
        change.y = player.transform.position.y - transform.position.y;
        float distance = change.magnitude;
        if (distance <= MIN_DIST || distance >= MAX_DIST)
            change = Vector3.zero;
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        myRigidBody.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
        );
    }
}
