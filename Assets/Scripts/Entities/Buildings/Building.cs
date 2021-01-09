using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Building : Entity
{
    [ReadOnly] public int level;

    protected NavMeshSurface2d navMesh;
    protected Interactable interactable;
    
    protected override void OnAwake()
    {
        navMesh = FindObjectOfType<NavMeshSurface2d>();
        interactable = GetComponentInChildren<Interactable>();
        PolygonCollider2D targetsPolygon = GetComponent<PolygonCollider2D>();
        if (targetsPolygon != null && !targetsPolygon.isActiveAndEnabled)
            AddTargets(targetsPolygon);
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
        Hide();
        //healthBar.transform.localScale = Vector3.zero;
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

    private void Hide()
    {
        gameObject.layer = Layers.DEFAULT;
        SilentBuildNavMesh();
        SetInteractibleActive(false);
        ShowHealthBar(false);
        OnHide();
    }
    protected virtual void OnHide() { }

    private void Show()
    {
        gameObject.layer = Layers.UNWALKABLE;
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
        healthBar.transform.localScale = show ? Vector3.one : Vector3.zero;
    }

    private void SilentBuildNavMesh()
    {
        Debug.unityLogger.logEnabled = false;
        navMesh.BuildNavMesh();
        Debug.unityLogger.logEnabled = true;
    }
}
