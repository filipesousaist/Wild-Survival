using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : SimpleBuilding
{
    private RhinosManager rhinosManager;
    private Fade fade;
    protected override void OnAwake()
    {
        base.OnAwake();
        rhinosManager = FindObjectOfType<RhinosManager>();
        fade = FindObjectOfType<Fade>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        Upgrade();
    }

    protected override bool IsPlayerTryingToInteract()
    {
        return playerNear && Input.GetAxisRaw("Vertical") > 0 && !interacting;
    }

    protected override void OnUpgrade()
    {
        // Maybe increase players' rested xp or tent's HP
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
