using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public struct Wave
{
    public EnemyAmount[] enemies;
    public Camp[] targetCamps;
}

[System.Serializable]
public struct EnemyAmount
{
    public GameObject prefab;
    public int n;
}

public class DefeatWavesMission : HelpArrowMission
{
    public Wave[] waves;

    public EnemiesManager enemiesManager;
    public WavesSpawnManager wavesSpawnManager;

    public GameObject roadblock;

    protected override void OnBegin()
    {
        enemiesManager.ChangeMode();
        helpArrow.GetComponent<Image>().color = Color.red;
    }

    protected override void OnFinish()
    {
        helpArrow.SetActive(false);
        enemiesManager.ChangeMode();
        roadblock.SetActive(false);
    }

    public override bool IsCompleted()
    {
        return wavesSpawnManager.currentWave == waves.Length;
    }

    public Camp[] GetTargetCamps()
    {
        return waves[wavesSpawnManager.currentWave].targetCamps;
    }

    public override string GetMessage()
    {
        int currentWave = wavesSpawnManager.currentWave;

        if (currentWave >= 0 && currentWave < waves.Length)
        {
            Enemy[] enemies = enemiesManager.GetAllEnemies();
            int enemiesAlive = enemies.Count((en) => en.wave == currentWave);
            int totalEnemies = CountWaveEnemies(waves[currentWave]);

            return "Wave " + (currentWave + 1) + " of " + waves.Length + ": " +
                (totalEnemies - enemiesAlive) + "/" + totalEnemies + " zombies defeated";
        }
        return "";
    }

    public override string GetFinishMessage()
    {
        return "The zombie ambush opened up a gap between the cliffs!";
    }

    public int CountWaveEnemies(Wave wave)
    {
        int count = 0;
        foreach (EnemyAmount enemyAmount in wave.enemies)
            count += enemyAmount.n;
        return count;
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
