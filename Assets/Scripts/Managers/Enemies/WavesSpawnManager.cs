using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WavesSpawnManager : SpawnManager
{
    private MissionsManager missionsManager;
    private DefeatWavesMission currentMission;
    [ReadOnly] public int currentWave;

    private Vector2[] spawnPoints;
    private Camp[] camps;
    private void Awake()
    {
        targetAI = new WavesTargetAI();
        missionsManager = FindObjectOfType<MissionsManager>();
        spawnPoints = GetComponent<GrindSpawnManager>().spawnPoints;
        camps = FindObjectsOfType<Camp>();
    }

    override public void OnEnterMode()
    {
        currentMission = (DefeatWavesMission) missionsManager.GetCurrentMission();
        currentWave = -1;
        StartCoroutine(UpdateEnemiesCo());
    }

    override public void OnExitMode()
    {
        enemiesManager.RemoveAllEnemies();
    }

    override protected void UpdateEnemies()
    {
        Enemy[] enemies = enemiesManager.GetAllEnemies();

        if (enemies.Length == 0 && (++currentWave) < currentMission.waves.Length)
        {
            Wave wave = currentMission.waves[currentWave];
            IEnumerable<Vector2> spawnPoints = GenerateSpawnPoints(wave);

            int i = 0;
            foreach (EnemyAmount enemyAmount in wave.enemies)
                for (int end = i + enemyAmount.n; i < end; i ++)
                    SpawnEnemy(spawnPoints.ElementAt(i), enemyAmount.prefab)
                        .GetComponent<Enemy>().wave = currentWave;
        }
    }

    private IEnumerable<Vector2> GenerateSpawnPoints(Wave wave)
    {
        return spawnPoints
            .OrderBy(point => DistanceToNearestCamp(wave.targetCamps, point))
            .Take(currentMission.CountWaveEnemies(wave))
            .OrderBy(point => Random.value);
    }

    private float DistanceToNearestCamp(Camp[] camps, Vector3 point)
    {
        return camps.OrderBy(camp => camp.Distance(point)).First().Distance(point);
    }
    /*
    protected Vector3[] GenerateSpawnPoints(Vector3 center, int n)
    {
        int sideLength = Mathf.CeilToInt(Mathf.Sqrt(n));
        Vector3[] coords = GenerateCoordsInSquare(center, sideLength);
        Vector3[] spawnPoints = new Vector3[n];

        for (int k = 0; k < n; k++)
        {
            int j = coords.Length - 1 - k;
            int i = Random.Range(0, j + 1);
            spawnPoints[k] = coords[i];
            coords[i] = coords[j];
        }

        return spawnPoints;
    }

    private Vector3[] GenerateCoordsInSquare(Vector3 center, int sideLength)
    {
        float offset = 0.5f * (sideLength - 1);
        Vector3 topLeft = new Vector3(center.x - offset, center.y - offset, 0);

        Vector3[] coords = new Vector3[sideLength * sideLength];
        for (int x = 0; x < sideLength; x++)
            for (int y = 0; y < sideLength; y++)
                coords[y * sideLength + x] = topLeft + new Vector3(x, y, 0);

        return coords;
    }
    */

    protected override EnemyTargetCriteria GetTargetCriteria()
    {
        return Random.Range(0, 2) == 0 ? EnemyTargetCriteria.building :
              (Random.Range(0, 2) == 0 ? EnemyTargetCriteria.health : EnemyTargetCriteria.distance);
    }
}
