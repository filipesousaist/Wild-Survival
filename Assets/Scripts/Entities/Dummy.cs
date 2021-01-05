using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Entity
{
    public new BoxCollider2D collider2D;

    public SpriteRenderer sprite;

    private float deadTime;

    private float disableTime = 5;

    private float waitTime;

    private float respawnTime = 5;

    private bool active = true;

    override protected void OnAwake() {
    }
    override protected void OnStart() {
        health = 1;
        healthBar.transform.localScale /= 3;
        healthBar.transform.localPosition += new Vector3(0, -1.3f);
    }

    override protected void OnDeath()
    {
        animator.SetBool("Dead", true);
        collider2D.enabled = false;
        gameObject.tag = "deadDummy";
        deadTime = Time.time;
    }

    public override void Knock(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        TakeDamage(damage);
    }

    // Update is called once per frame
    void Update()
    {
        if (active && gameObject.CompareTag("deadDummy") &&  Time.time - deadTime > disableTime)
        {
            active = false;
            Color temp = sprite.color;
            temp.a = 0;
            sprite.color = temp;
            waitTime = Time.time;
        }

        if (!active && Time.time - waitTime > respawnTime) {
            active = true;
            animator.SetBool("Dead", false);
            collider2D.enabled = enabled;
            Color temp = sprite.color;
            temp.a = 1;
            sprite.color = temp;
            gameObject.tag = "dummy";
            health = 1;
        }
    }
}
