using System.Collections;
using UnityEngine;

public class RestInTent : Interactable
{
    private RhinosManager rhinosManager;
    private Fade fade;
    protected override void OnAwake()
    {
        rhinosManager = FindObjectOfType<RhinosManager>();
        fade = FindObjectOfType<Fade>();
    }

    protected override bool IsPlayerTryingToInteract()
    {
        return isInteractable && Input.GetAxisRaw("Vertical") > 0 && !interacting;
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
