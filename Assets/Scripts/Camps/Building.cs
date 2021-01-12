using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Building : Entity
{
    [ReadOnly] public int level;

    protected NavMeshSurface2d navMesh;
    protected Interactable interactable;

    private GameObject signGameObject;

    private Vector3 healthBarScale;

    public List<MatDict> materials;

    protected override void OnAwake()
    {
        navMesh = FindObjectOfType<NavMeshSurface2d>();
        interactable = GetInteractable();
        AddTargets(GetComponent<PolygonCollider2D>());
        signGameObject = GetComponentInChildren<Sign>().gameObject;
    }

    private Interactable GetInteractable()
    {
        foreach (Interactable possibleInteractible in GetComponentsInChildren<Interactable>())
            if (possibleInteractible.GetType() != typeof(Sign))
                return possibleInteractible;
        return null;
    }

    private void AddTargets(PolygonCollider2D targetsPolygon)
    {
        foreach (Vector2 targetPoint in targetsPolygon.points)
        {
            BuildingTarget newTarget = gameObject.AddComponent<BuildingTarget>();
            newTarget.position = targetPoint;
        }
    }

    protected override void OnStart()
    {
        level = 0;
        Vector3 localScale = transform.localScale;
        healthBarScale = new Vector3(1 / localScale.x, 1 / localScale.y, 1);
        Hide();
    }

    protected override void OnDeath()
    {
        Hide();
    }

    private void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.L))
            Upgrade();
        if (Input.GetKeyDown(KeyCode.K))
            OnDeath();
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void Upgrade()
    {
        if ((level++) == 0)
            Show();
        OnUpgrade();
        health = maxHealth; // In case maxHealth increased in OnUpgrade
    }
    protected virtual void OnUpgrade() { }

    public void Repair()
    {
        Show();
        OnRepair();
        health = maxHealth;
    }

    protected virtual void OnRepair() { }

    private void Hide()
    {
        gameObject.layer = Layers.DEFAULT;
        signGameObject.SetActive(true);
        SilentBuildNavMesh();
        SetInteractibleActive(false);
        ShowHealthBar(false);
        OnHide();
    }
    protected virtual void OnHide() { }

    private void Show()
    {
        gameObject.layer = Layers.UNWALKABLE;
        signGameObject.SetActive(false);
        SilentBuildNavMesh();
        SetInteractibleActive(true);
        ShowHealthBar(true);
        OnShow();
    }
    protected virtual void OnShow() { }

    public override void Knock(float knockTime, float damage)
    {
        TakeDamage(damage);
    }

    private void SetInteractibleActive(bool active)
    {
        if (interactable != null)
            interactable.gameObject.SetActive(active);
    }

    protected virtual void ShowHealthBar(bool show)
    {
        healthBar.transform.localScale = show ? healthBarScale : Vector3.zero;
    }

    private void SilentBuildNavMesh()
    {
        Debug.unityLogger.logEnabled = false;
        navMesh.BuildNavMesh();
        Debug.unityLogger.logEnabled = true;
    }

    public bool IsBuilt()
    {
        return gameObject.layer == Layers.UNWALKABLE;
    }
}
