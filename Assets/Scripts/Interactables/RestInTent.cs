using System.Collections;
using UnityEngine;

public class RestInTent : Interactable
{
    private EnemiesManager enemiesManager;
    private RhinosManager rhinosManager;
    private Fade fade;
    protected override void OnAwake()
    {
        enemiesManager = FindObjectOfType<EnemiesManager>();
        rhinosManager = FindObjectOfType<RhinosManager>();
        fade = FindObjectOfType<Fade>();
    }

    protected override bool IsPlayerTryingToInteract()
    {
        return isInteractable && Input.GetAxisRaw("Vertical") > 0 && 
            enemiesManager.CanUseTent() && !interacting;
    }

    protected override IEnumerator OnInteract()
    {
        PlayerMovement currentPlayerMov = activistsManager.GetCurrentPlayerMovement();

        currentPlayerMov.inputEnabled = false;
        yield return StartCoroutine(fade.FadeToBlack());

        // Restaurar vida
        activistsManager.HealAll();
        rhinosManager.HealAll();

        currentPlayerMov.animator.SetFloat("moveY", -1);

        yield return StartCoroutine(fade.FadeToClear());

        currentPlayerMov.inputEnabled = true;
    }
}
