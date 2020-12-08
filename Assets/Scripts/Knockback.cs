using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Movement movement = this.GetComponentInParent<Movement>();
        
        if (other.gameObject.CompareTag("enemy") && Time.time - movement.lastAttackTime > 0.1)
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                    hit.AddForce(difference, ForceMode2D.Impulse);
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                    movement.lastAttackTime = Time.time;

                }
                
            }
        }
    }
}
