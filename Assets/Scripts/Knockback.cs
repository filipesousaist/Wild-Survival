using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float baseDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityMovement movement = GetComponentInParent<EntityMovement>();
        
        if (other.isTrigger &&
            AreOpponents(GetComponent<Collider2D>(), other) && 
            ! movement.attackedRecently)
        {
            Rigidbody2D hit = other.attachedRigidbody;
            if (hit != null)
            {  
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;

                hit.AddForce(difference, ForceMode2D.Impulse);

                Entity entity = GetComponentInParent<Entity>();
                Entity otherEntity = other.attachedRigidbody.GetComponent<Entity>();
                otherEntity.Knock(hit, knockTime, baseDamage * entity.baseAttack);
                movement.attackedRecently = true;
            }
        }
    }

    private bool AreOpponents(Collider2D col1, Collider2D col2)
    {
        bool is1Evil = col1.attachedRigidbody.CompareTag("enemy");
        bool is2Evil = col2.attachedRigidbody.CompareTag("enemy");

        return is1Evil ^ is2Evil;
    }
}
