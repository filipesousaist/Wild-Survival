﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnManager : MonoBehaviour
{
    protected EnemiesManager enemiesManager;
    protected GameObject textObj;
    protected GameObject enemiesObj;
    protected GameObject[] prefabs;
    protected GameObject helpArrow;

    // Start is called before the first frame update
    public void Init(EnemiesManager manager)
    {
        enemiesManager = manager;
        textObj = manager.textObj;
        enemiesObj = manager.enemiesObj;
        prefabs = manager.prefabs;
        helpArrow = manager.helpArrow;
    }

    public abstract void OnEnterMode();
    public abstract void OnExitMode();

    public IEnumerator UpdateAll()
    {
        // Wait until end of frame to make sure objects were properly destroyed
        yield return null; 
        UpdateEnemies();
        UpdateText();
    }

    protected abstract void UpdateEnemies();
    public abstract void UpdateText();

    protected GameObject SpawnEnemy(Vector2 position)
    {
        int enemyIndex = (Random.value > 0.05) ? 0 : 1;
        GameObject newEnemy = Instantiate(prefabs[enemyIndex]);
        newEnemy.transform.parent = enemiesObj.transform;
        newEnemy.transform.position = position;
        return newEnemy;
    }
}
