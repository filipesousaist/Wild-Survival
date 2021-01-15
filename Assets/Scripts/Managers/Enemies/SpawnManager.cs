using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class SpawnManager : MonoBehaviour
{
    protected EnemiesManager enemiesManager;
    [ReadOnly] public EnemyTargetAI targetAI;
    protected GameObject textObj;
    protected GameObject enemiesObj;
    protected GameObject[] prefabs;

    public void Init(EnemiesManager manager)
    {
        enemiesManager = manager;
        textObj = manager.textObj;
        enemiesObj = manager.enemiesObj;
        prefabs = manager.prefabs;
    }

    public abstract void OnEnterMode();
    public abstract void OnExitMode();

    public IEnumerator UpdateEnemiesCo()
    {
        // Wait until end of frame to make sure objects were properly destroyed
        yield return null; 
        UpdateEnemies();
    }

    protected abstract void UpdateEnemies();

    protected GameObject SpawnEnemy(Vector2 position)
    {
        int strongIncr = (Random.value > 0.05) ? 0 : 1;
        int enemyIndex = 2 * Random.Range(0, 6) + strongIncr;
        return SpawnEnemy(position, prefabs[enemyIndex]);
    }

    protected GameObject SpawnEnemy(Vector2 position, GameObject prefab)
    {
        GameObject newEnemy = Instantiate(prefab);
        newEnemy.transform.parent = enemiesObj.transform;
        newEnemy.GetComponent<NavMeshAgent>().Warp(position);
        newEnemy.GetComponent<Enemy>().targetCriteria = GetTargetCriteria();
        return newEnemy;
    }

    protected abstract EnemyTargetCriteria GetTargetCriteria();
}
