using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RhinoState
{
    walk,
    attack
}
public class RhinoMovement : MonoBehaviour
{
    public RhinoState currentState;
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    private int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentState = RhinoState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != RhinoState.attack)
        {
            Debug.Log("attack" + i++);
            StartCoroutine(AttackCo());
        }
        else if (currentState == RhinoState.walk)
        {
            UpdateAnimationAndMove();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player" || collision.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }

    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = RhinoState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.62f);
        currentState = RhinoState.walk;
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
