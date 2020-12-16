using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EnemiesManager : MonoBehaviour
{
    private int currentWave = -1;
    public int[] waves;
    public Vector2[] baseSpawnPoints;
    public GameObject[] prefabs;
    public GameObject wavesText;

    void Start()
    {
        UpdateWavesText();
    }

    void FixedUpdate()
    {
        if (currentWave < waves.Length)
            UpdateEnemies();
    }

    private void UpdateEnemies()
    {
        Enemy[] enemies = GetAllEnemies();

        if (enemies.Length == 0 && (++ currentWave) < waves.Length)
        {
            Vector3 baseSpawn = new Vector3(baseSpawnPoints[currentWave].x, baseSpawnPoints[currentWave].y, 0);
            Vector3[] spawnPoints = GenerateSpawnPoints(baseSpawn, waves[currentWave]);

            for (int i = 0; i < waves[currentWave]; i ++)
            {
                int enemyIndex = (Random.value > 0.05) ? 0 : 1;
                GameObject newEnemy = Instantiate(prefabs[enemyIndex]);
                newEnemy.transform.parent = GameObject.Find("Enemies").transform;
                newEnemy.transform.position = spawnPoints[i];
                newEnemy.GetComponent<Enemy>().wave = currentWave;
            }
        }

        UpdateWavesText();
    }

    private Enemy[] GetAllEnemies()
    {
        return GameObject.Find("Enemies").GetComponentsInChildren<Enemy>();
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

    public void UpdateWavesText()
    {
        string newText;
        if (currentWave < 0)
            newText = "Welcome to Wild Survival!";
        else if (currentWave < waves.Length)
        {
            Enemy[] enemies = GetAllEnemies();
            List<Enemy> currentWaveEnemies = new List<Enemy>();

            foreach (Enemy en in enemies)
                if (en.wave == currentWave)
                    currentWaveEnemies.Add(en);

            newText = "Wave " + (currentWave + 1) + " of " + waves.Length + ":";
            newText += " " + currentWaveEnemies.Count() + "/" + waves[currentWave] + " zombies left";
        }
        else
            newText = "All zombies defeated! :)";

        wavesText.GetComponent<Text>().text = newText;
    }
}
