using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatWavesMission : Mission
{
    private EnemiesManager enemiesManager;
    private WavesSpawnManager wavesSpawnManager;

    private void Awake()
    {
        enemiesManager = FindObjectOfType<EnemiesManager>();
        wavesSpawnManager = FindObjectOfType<WavesSpawnManager>();
    }

    public override void OnBegin()
    {
        enemiesManager.ChangeMode();
    }

    public override bool IsCompleted()
    {
        return wavesSpawnManager.currentWave == wavesSpawnManager.waves.Length;
    }
    public override string GetMessage()
    {
        int[] waves = wavesSpawnManager.waves;
        int currentWave = wavesSpawnManager.currentWave;

        if (currentWave >= 0 && currentWave < wavesSpawnManager.waves.Length)
        {
            Enemy[] enemies = enemiesManager.GetAllEnemies();
            int waveEnemiesAlive = 0;

            foreach (Enemy en in enemies)
                if (en.wave == currentWave)
                    waveEnemiesAlive++;

            return "Wave " + (currentWave + 1) + " of " + waves.Length + ": " +
                (waves[currentWave] - waveEnemiesAlive) + "/" + waves[currentWave] +
                " zombies defeated";
        }
        return "";
    }
}
