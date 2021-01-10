using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public Vector3 center;
    public Vector2[] rhinoFleeTarget;

    private BuildingTarget[] buildingTargets;

    private void Start()
    {
        buildingTargets = GetComponentsInChildren<BuildingTarget>(); // This must occur after Awake, because it's when building targets are created
    }

    public IEnumerable<BuildingTarget> GetBuildingTargets()
    {
        return buildingTargets;
    }

    public float Distance(Vector3 position)
    {
        return (center - position).magnitude;
    }
}
