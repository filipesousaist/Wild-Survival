using System.Collections;
using UnityEngine;
public enum PlayerState
{
    walk,
    attack
}

public class PlayerMovement : EntityMovement
{
    public PlayerState currentState;

    // Start is called before the first frame update
    override protected void OnStart()
    {
        currentState = PlayerState.walk;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Vector3.zero;
        difference.x = Input.GetAxisRaw("Horizontal");
        difference.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
            if (difference.magnitude > 0)
            {
                MoveCharacter(difference);
            }
            else
            {
                animator.SetBool("moving", false);
            }

            UpdateAnimation(difference);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("moving", false);
        animator.SetBool("attacking",true);
        ChangeState(PlayerState.attack);
        yield return null;
        animator.SetBool("attacking",false);
        yield return new WaitForSeconds(.33f);
        ChangeState(PlayerState.walk);
        attackedRecently = false;
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
