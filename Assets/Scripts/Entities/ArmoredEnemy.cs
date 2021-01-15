using System.Collections;
using UnityEngine;

public class ArmoredEnemy : Enemy
{
    private ParticleSystem ps;

    private Vector2 lastDirection;

    protected override void OnAwake()
    {
        base.OnAwake();
        ps = GetComponent<ParticleSystem>();

    }

    protected override void OnStart()
    {
        base.OnStart();
        ps.Stop();
        lastDirection = Vector2.zero;
    }

    private void Update()
    {
        Vector2 dir = GetSpriteDirection();
        if (dir != Vector2.zero)
            lastDirection = dir;
            
    }

    public override void Knock(float knockTime, float damage, Collider2D attackerCol)
    {
        Vector2 difference = transform.position - attackerCol.transform.position;

        Vector2 product = lastDirection * difference;
        bool attackFromBehind = product.x + product.y > 0;
        if (attackFromBehind)
            StartCoroutine(Bleed());
        else
            damage /= 10;

        base.Knock(knockTime, damage, attackerCol);
    }

    IEnumerator Bleed()
    {
        ps.Play();
        Debug.Log("From behind");
        yield return new WaitForSeconds(.4f);
        ps.Stop();
    }

    public Vector2 GetSpriteDirection()
    {
        float x = animator.GetFloat("moveX");
        float y = animator.GetFloat("moveY");
        return new Vector2(x, y);
    }
}
