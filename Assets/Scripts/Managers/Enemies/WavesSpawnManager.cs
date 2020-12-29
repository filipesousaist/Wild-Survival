using UnityEngine;
using UnityEngine.UI;

public class WavesSpawnManager : SpawnManager
{
    private int currentWave;
    public int[] waves;
    public Vector2[] spawnPoints;

    override public void OnEnterMode()
    {
        currentWave = -1;
        StartCoroutine(UpdateAll());
    }

    override public void OnExitMode()
    {
        enemiesManager.RemoveAllEnemies();
        helpArrow.SetActive(false);
    }

    override protected void UpdateEnemies()
    {
        Enemy[] enemies = enemiesManager.GetAllEnemies();

        if (enemies.Length == 0 && (++currentWave) < waves.Length)
        {
            Vector3 baseSpawn = new Vector3(this.spawnPoints[currentWave].x, this.spawnPoints[currentWave].y, 0);
            Vector3[] spawnPoints = GenerateSpawnPoints(baseSpawn, waves[currentWave]);

            for (int i = 0; i < waves[currentWave]; i++)
                SpawnEnemy(spawnPoints[i])
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

    override public void UpdateText()
    {
        string newText = "";

        if (currentWave >= 0)
        {
            if (currentWave < waves.Length)
            {
                Enemy[] enemies = enemiesManager.GetAllEnemies();
                int waveEnemiesAlive = 0;

                foreach (Enemy en in enemies)
                    if (en.wave == currentWave)
                        waveEnemiesAlive++;

                newText = "Wave " + (currentWave + 1) + " of " + waves.Length + ":";
                newText += " " + waveEnemiesAlive + "/" + waves[currentWave] + " zombies left";
            }
            else
                newText = "All zombies defeated! :)";
        }
        

        textObj.GetComponent<Text>().text = newText;
    }
}
