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

    protected override void OnBegin()
    {
        enemiesManager.ChangeMode();
    }

    protected override void OnFinish()
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

    public override void UpdateHelpArrow(GameObject helpArrow)
    {
        List<EnemyMovement> invisibleEnemyMovs = enemiesManager.GetInisibleEnemyMovements();

        int numInvisble = invisibleEnemyMovs.Count;
        int numVisible = enemiesManager.GetAllEnemies().Length - numInvisble;
        if (numVisible == 0 && numInvisble > 0)
        {
            Vector2 difference = enemiesManager.GetNearestToCamera(invisibleEnemyMovs).position - Camera.main.transform.position;

            float angle = Vector3.SignedAngle(Vec3.E_Y, difference.normalized, Vec3.E_Z);
            helpArrow.transform.eulerAngles =
                new Vector3(0, 0, angle);
            helpArrow.transform.localPosition = new Vector3(
                -Window.QUARTER_H * Mathf.Sin(angle * Mathf.Deg2Rad),
                Window.QUARTER_H * Mathf.Cos(angle * Mathf.Deg2Rad),
                0);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);
    }
}
