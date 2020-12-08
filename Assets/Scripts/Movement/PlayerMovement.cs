using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    walk,
    attack
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public PlayerState currentState;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            //Debug.Log("attack" + i++);
            StartCoroutine(AttackCo());
        }
        else if(currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove();
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FadeObstacle") && other.isTrigger)
        {

            var fadeScript = Canvas.FindObjectOfType<Fade>();
            
            yield return StartCoroutine(fadeScript.FadeToBlack());
            yield return StartCoroutine(fadeScript.FadeToClear());

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rhino" || collision.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            this.myRigidBody.velocity = Vector2.zero;
        }
    }

    
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking",true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking",false);
        yield return new WaitForSeconds(.33f);
        currentState = PlayerState.walk;
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
