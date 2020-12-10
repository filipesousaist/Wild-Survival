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
    public bool inputEnabled;


    // Start is called before the first frame update
    override protected void OnStart()
    {
        currentState = PlayerState.walk;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputEnabled) { 
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
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FadeObstacle") && other.isTrigger && Input.GetAxisRaw("Vertical") > 0)
        {

            var fadeScript = Canvas.FindObjectOfType<Fade>();
            
            yield return StartCoroutine(fadeScript.FadeToBlack());

            //codigo de restaurar vida
            this.GetComponent<Player>().health = this.GetComponent<Player>().maxHealth.value;

            animator.SetFloat("moveY", -1);

            yield return StartCoroutine(fadeScript.FadeToClear());

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
