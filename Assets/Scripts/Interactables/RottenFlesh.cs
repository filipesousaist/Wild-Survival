using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenFlesh : Interactable
{
    private EnemiesManager enemiesManager;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem.MainModule psMain;
    protected override void OnAwake()
    {
        base.OnAwake();
        enemiesManager = FindObjectOfType<EnemiesManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        psMain = GetComponent<ParticleSystem>().main;
    }
    override protected void OnStart()
    {
        once = true;
    }
    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();

        spriteRenderer.color = Colors.OPAQUE;
        psMain.startLifetime = 2;

        yield return new WaitForSeconds(2);

        enemiesManager.ChangeMode();

        yield return new WaitForSeconds(120);

        Destroy(gameObject);
    }

    public override string GetInteractText()
    {
        return "Inspect";
    }
}
