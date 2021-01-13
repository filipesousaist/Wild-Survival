using UnityEngine;

public class WavesSpawnManager : SpawnManager
{
    private MissionsManager missionsManager;
    private DefeatWavesMission currentMission;
    [ReadOnly] public int currentWave;
    
    private void Awake()
    {
        targetAI = new WavesTargetAI();
        missionsManager = FindObjectOfType<MissionsManager>();
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
            Vector3[] spawnPoints = GenerateSpawnPoints(wave.spawnPoint, wave.amount);

            foreach (Vector3 point in spawnPoints)
                SpawnEnemy(point)
                    .GetComponent<Enemy>().wave = currentWave;
        }
    }

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

    protected override EnemyTargetCriteria GetTargetCriteria()
    {
        return Random.Range(0, 2) == 0 ? EnemyTargetCriteria.building :
              (Random.Range(0, 2) == 0 ? EnemyTargetCriteria.health : EnemyTargetCriteria.distance);
    }
}
