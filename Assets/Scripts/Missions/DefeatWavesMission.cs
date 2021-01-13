using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public struct Wave
{
    public int amount;
    public Vector2 spawnPoint;
}

public class DefeatWavesMission : HelpArrowMission
{
    public Wave[] waves;

    public EnemiesManager enemiesManager;
    public WavesSpawnManager wavesSpawnManager;

    protected override void OnBegin()
    {
        enemiesManager.ChangeMode();
        helpArrow.GetComponent<Image>().color = Color.red;
    }

    protected override void OnFinish()
    {
        helpArrow.SetActive(false);
        enemiesManager.ChangeMode();
    }

    public override bool IsCompleted()
    {
        return wavesSpawnManager.currentWave == waves.Length;
    }
    public override string GetMessage()
    {
        int currentWave = wavesSpawnManager.currentWave;

        if (currentWave >= 0 && currentWave < waves.Length)
        {
            Enemy[] enemies = enemiesManager.GetAllEnemies();
            int enemiesAlive = enemies.Count((en) => en.wave == currentWave);
            int totalEnemies = waves[currentWave].amount;

            return "Wave " + (currentWave + 1) + " of " + waves.Length + ": " +
                (totalEnemies - enemiesAlive) + "/" + waves[currentWave].amount +
                " zombies defeated";
        }
        return "";
    }

    public override void UpdateHelpArrow()
    {
        List<EnemyMovement> invisibleEnemyMovs = enemiesManager.GetInisibleEnemyMovements();

        int numInvisble = invisibleEnemyMovs.Count;
        int numVisible = enemiesManager.GetAllEnemies().Length - numInvisble;
        if (numVisible == 0 && numInvisble > 0)
        {
            Vector2 difference = enemiesManager.GetNearestToCamera(invisibleEnemyMovs).position - Camera.main.transform.position;
            SetArrowPosition(difference);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);
    }
}
