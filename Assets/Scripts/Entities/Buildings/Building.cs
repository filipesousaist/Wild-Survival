using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Building : Entity
{
    protected int level;

    protected NavMeshSurface2d navMesh;
    protected Interactable interactable;
    
    protected override void OnAwake()
    {
        navMesh = FindObjectOfType<NavMeshSurface2d>();
        interactable = GetComponentInChildren<Interactable>();
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
        //DEBUG
        if (Input.GetKeyDown(KeyCode.L))
            Upgrade();
        if (Input.GetKeyDown(KeyCode.K))
            OnDeath();
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
        SilentBuildNavMesh();
        SetInteractibleActive(false);
        OnHide();
    }
    protected abstract void OnHide();

    private void Show()
    {
        gameObject.layer = Layers.UNWALKABLE;
        SilentBuildNavMesh();
        SetInteractibleActive(true);
        OnShow();
    }
    protected abstract void OnShow();

    private void SetInteractibleActive(bool active)
    {
        if (interactable != null)
            interactable.gameObject.SetActive(active);
    }

    private void SilentBuildNavMesh()
    {
        Debug.unityLogger.logEnabled = false;
        navMesh.BuildNavMesh();
        Debug.unityLogger.logEnabled = true;
    }
}
