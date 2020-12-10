using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesManager : MonoBehaviour
{
    private int currentWave = -1;
    public int[] waves;
    public Vector2[] baseSpawnPoints;
    public GameObject[] prefabs;

    void Start()
    {
        
    }

    void Update()
    {
        if (currentWave < waves.Length)
            UpdateEnemies();
    }

    private void UpdateEnemies()
    {
        GameObject enemiesObject = GameObject.Find("Enemies");
        Enemy[] enemies = enemiesObject.GetComponentsInChildren<Enemy>();

        if (enemies.Length == 0 && (++ currentWave) < waves.Length)
        {
            Vector3 baseSpawn = new Vector3(baseSpawnPoints[currentWave].x, baseSpawnPoints[currentWave].y, 0);
            Vector3[] spawnPoints = GenerateSpawnPoints(baseSpawn, waves[currentWave]);

            for (int i = 0; i < waves[currentWave]; i ++)
            {
                GameObject newEnemy = Instantiate(prefabs[0]);
                newEnemy.transform.parent = enemiesObject.transform;
                newEnemy.transform.position = spawnPoints[i];
            }
        }
    }

    private Vector3[] GenerateSpawnPoints(Vector3 center, int n)
    {
        int sideLength = Mathf.CeilToInt(Mathf.Sqrt(n));
        Vector3[] coords = GenerateCoordsInSquare(center, sideLength);
        Vector3[] spawnPoints = new Vector3[n];

        for (int k = 0; k < n; k ++)
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
}
