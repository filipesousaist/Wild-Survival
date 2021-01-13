using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WavesTargetAI : EnemyTargetAI
{
    private readonly Camp[] camps;
    public WavesTargetAI()
    {
        camps = Object.FindObjectsOfType<Camp>();
    }

    protected override Vector3 Target(EnemyMovement enemyMov)
    {
        Vector3 diffToNearestChar = TargetByDistance(enemyMov, GetPossibleTargets(enemyMov));
        if (diffToNearestChar.magnitude <= ALWAYS_ATTACK_DIST)
            return diffToNearestChar;

        if (enemyMov.GetComponent<Enemy>().targetCriteria == EnemyTargetCriteria.building)
        {
            IEnumerable<IEnemyTarget> buildingTargets = GetPossibleBuildingTargets(enemyMov);
            return buildingTargets.Any() ? TargetByDistance(enemyMov, buildingTargets) :
                                           diffToNearestChar;
        }
        return base.Target(enemyMov);
    }

    protected override IEnumerable<IEnemyTarget> GetPossibleTargets(EnemyMovement enemyMov)
    {
        return base.GetPossibleTargets(enemyMov).Concat(GetPossibleBuildingTargets(enemyMov));
    }

    private IEnumerable<BuildingTarget> GetPossibleBuildingTargets(EnemyMovement enemyMov)
    {
        Vector3 enemyPos = enemyMov.transform.position;
        IOrderedEnumerable<Camp> campsByDistance = camps.OrderBy(camp => camp.Distance(enemyPos));

        foreach (Camp camp in campsByDistance)
        {
            IEnumerable<BuildingTarget> possibleTargets = camp.GetBuildingTargets().Where((target) => target.CanBeTargeted());
            if (possibleTargets.Any())
                return possibleTargets;
        }
        return new BuildingTarget[0];
    } 
}
