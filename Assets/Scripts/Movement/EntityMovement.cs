using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    public Animator animator;
    protected Rigidbody2D myRigidBody;
    protected Vector3 change;
    private Entity entity;

    [ReadOnly] public bool attackedRecently;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        entity = GetComponent<Entity>();
        attackedRecently = false;

        OnStart();
    }

    abstract protected void OnStart();

    public virtual IEnumerator KnockCo(Rigidbody2D myRigidBody, float knockTime)
    {
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
        }
    }

    public virtual void Flee()
    {

    }
    protected virtual void UpdateAnimation(Vector3 difference)
    {
        if (difference != Vector3.zero)
        {
            animator.SetFloat("moveX", difference.x);
            animator.SetFloat("moveY", difference.y);
        }
    }

    protected void MoveCharacter(Vector3 difference)
    {
        animator.SetBool("moving", true);
        myRigidBody.MovePosition(
            transform.position + difference.normalized * speed * Time.deltaTime
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("player") || collision.collider.CompareTag("rhino") || collision.collider.CompareTag("enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            myRigidBody.velocity = Vector2.zero;
        }
    }

    protected string EnableCorrectHitbox(Vector3 difference)
    {
        float y = difference.y;
        float x = difference.x;
        string correctHitbox;
        if (y > x)
        {
            if (y > -x)
                correctHitbox = "up hit box";
            else
                correctHitbox = "left hit box";
        }
        else
        {
            if (y > -x)
                correctHitbox = "right hit box";
            else
                correctHitbox = "down hit box";
        }

        string[] allHitboxes = { "right hit box", "up hit box", "left hit box", "down hit box" };
        foreach (string name in allHitboxes)
        {
            transform.Find(name).gameObject.SetActive(name == correctHitbox);
        }

        return correctHitbox;
    }

    protected void DisableHitbox(string name)
    {
        transform.Find(name).gameObject.SetActive(false);
    }
}
