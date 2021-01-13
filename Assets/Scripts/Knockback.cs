using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // "this" is a hitbox, and "other" must be a hurtbox
        EntityMovement movement = GetComponentInParent<EntityMovement>();
        Entity entity = GetComponentInParent<Entity>();
        Entity otherEntity = other.GetComponent<Entity>();

        if (other.isTrigger && !movement.attackedRecently)
        {
            if (otherEntity == null) // Can be a building
                otherEntity = other.GetComponentInParent<Entity>();
            if (otherEntity != null && Entity.AreOpponents(entity, otherEntity))
            {
                Vector2 difference = other.transform.position - transform.position;

                EntityMovement otherMovement = otherEntity.GetComponent<EntityMovement>();
                if (otherMovement != null)
                    otherMovement.velocity = difference.normalized * thrust;
                otherEntity.Knock(knockTime, entity.GetAttack(), entity);
                movement.attackedRecently = true;
            }
        }
    }
}
