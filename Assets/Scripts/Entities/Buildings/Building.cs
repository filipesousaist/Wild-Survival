using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Building : Entity
{
    protected int level;

    protected bool playerNear = false;
    protected bool interacting = false;

    protected ActivistsManager activistsManager;
    protected NavMeshSurface2d navMesh;
    
    protected override void OnAwake()
    {
        navMesh = FindObjectOfType<NavMeshSurface2d>();
        activistsManager = FindObjectOfType<ActivistsManager>();
    }

    protected override void OnStart()
    {
        level = 0;
        Hide();
        healthBar.transform.localScale = Vector3.zero;
    }

    protected override void OnDeath()
    {
        if (level > 0 && (-- level) == 0)
            Hide();
    }

    private void Update()
    {
        if (IsPlayerTryingToInteract())
            StartCoroutine(InteractCo());
        //DEBUG
        if (Input.GetKeyDown(KeyCode.L))
            Upgrade();
        if (Input.GetKeyDown(KeyCode.K))
            OnDeath();
    }

    protected virtual bool IsPlayerTryingToInteract()
    {
        return playerNear && Input.GetKeyDown(KeyCode.E) && !interacting;
    }

    public void Upgrade()
    {
        if ((level++) == 0)
            Show();
        OnUpgrade();
        health = maxHealth; // In case maxHealth increased in OnUpgrade
    }
    protected abstract void OnUpgrade();

    private void Hide()
    {
        gameObject.layer = Layers.DEFAULT;
        navMesh.BuildNavMesh();
        OnHide();
    }
    protected abstract void OnHide();

    private void Show()
    {
        gameObject.layer = Layers.UNWALKABLE;
        navMesh.BuildNavMesh();
        OnShow();
    }
    protected abstract void OnShow();

    private IEnumerator InteractCo()
    {
        interacting = true;
        yield return OnInteract();
        interacting = false;
    }

    protected virtual IEnumerator OnInteract()
    {
        yield return null;
    }
}
