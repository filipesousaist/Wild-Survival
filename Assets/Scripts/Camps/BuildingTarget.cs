using UnityEngine;

public class BuildingTarget : MonoBehaviour, IEnemyTarget
{
    public Vector2 position;

    private Building building;

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    public Vector3 GetPosition()
    {
        return transform.position + (Vector3) position;
    }

    public float GetHealthFraction()
    {
        return building.health / building.maxHealth;
    }

    public bool CanBeTargeted()
    {
        return building.level > 0 && building.health > 0;   
    }
}
