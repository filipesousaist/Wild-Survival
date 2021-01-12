using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMission : Mission
{
    public Building buildingType;
    public override bool IsCompleted()
    {
        foreach (Building building in FindObjectsOfType(buildingType.GetType()))
            if (building.IsBuilt())
                return true;
        return false;
    }

    public override string GetMessage()
    {
        int num = IsCompleted() ? 1 : 0;
        return "Build or repair " + buildingType.entityName + ": " + num + "/1";
    }
}
