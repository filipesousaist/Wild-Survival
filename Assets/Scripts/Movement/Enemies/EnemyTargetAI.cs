using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargetAI
{
    protected static readonly float ALWAYS_ATTACK_DIST = 3;
    public Vector3 Update(EnemyMovement enemyMov, float deltaTime)
    {
        enemyMov.updateTargetTime += deltaTime;
        if (enemyMov.updateTargetTime >= EnemyMovement.UPDATE_TARGET_PERIOD)
        {
            enemyMov.updateTargetTime -= EnemyMovement.UPDATE_TARGET_PERIOD;
            return Target(enemyMov);
        }
        return enemyMov.target.GetPosition() - enemyMov.transform.position;
    }

    protected virtual Vector3 Target(EnemyMovement enemyMov)
    {
        IEnumerable<IEnemyTarget> possibleTargets = GetPossibleTargets(enemyMov);
        Vector3 diffToNearestChar = TargetByDistance(enemyMov, possibleTargets);
        if (diffToNearestChar.magnitude <= ALWAYS_ATTACK_DIST)
            return diffToNearestChar;

        EnemyTargetCriteria criteria = enemyMov.GetComponent<Enemy>().targetCriteria;
        return criteria == EnemyTargetCriteria.health ? TargetByHealth(enemyMov, possibleTargets)
                                                      : diffToNearestChar;
    }

    protected virtual IEnumerable<IEnemyTarget> GetPossibleTargets(EnemyMovement enemyMov)
    {
        Character[] players = Object.FindObjectsOfType<Player>();
        Character[] rhinos = Object.FindObjectsOfType<Rhino>();
        IEnumerable<Character> possibleTargets = players.Concat(rhinos);

        return possibleTargets.Where((ch) => ch.GetComponent<EntityMovement>().CanBeTargeted());
    }

    protected Vector3 TargetByDistance(EnemyMovement enemyMov, IEnumerable<IEnemyTarget> possibleTargets)
    {
        if (possibleTargets.Any())
        {
            Vector3 enemyPos = enemyMov.transform.position;
            enemyMov.target = possibleTargets.First();
            Vector3 difference = enemyMov.target.GetPosition() - enemyPos;

            foreach (IEnemyTarget possibleTarget in possibleTargets.Skip(1))
            {
                Vector3 newDifference = possibleTarget.GetPosition() - enemyPos;
                if (newDifference.magnitude < difference.magnitude)
                {
                    enemyMov.target = possibleTarget;
                    difference = newDifference;
                }
            }
            return difference;
        }
        else
        {
            enemyMov.target = null;
            return Vector3.zero;
        }
    }

    protected Vector3 TargetByHealth(EnemyMovement enemyMov, IEnumerable<IEnemyTarget> possibleTargets)
    {
        if (possibleTargets.Any())
        {
            enemyMov.target = possibleTargets.First();
            float healthFrac = enemyMov.target.GetHealthFraction();

            foreach (IEnemyTarget possibleTarget in possibleTargets.Skip(1))
            {
                float newHealthFrac = possibleTarget.GetHealthFraction();
                if (newHealthFrac < healthFrac)
                {
                    enemyMov.target = possibleTarget;
                    healthFrac = newHealthFrac;
                }
            }
            return enemyMov.target.GetPosition() - enemyMov.transform.position;
        }
        else
        {
            enemyMov.target = null;
            return Vector3.zero;
        }
    }
}
