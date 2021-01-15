using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum EnemiesMode
{
    grind,
    waves
}
public class EnemiesManager : MonoBehaviour
{
    private EnemiesMode mode;

    public WavesSpawnManager wavesSpawnManager;
    public GrindSpawnManager grindSpawnManager;
    private Dictionary<EnemiesMode, SpawnManager> spawnManagers;

    [ReadOnly] public GameObject enemiesObj;
    public GameObject textObj;

    public GameObject[] prefabs;

    private void Awake()
    {
        enemiesObj = GameObject.Find("Enemies");
        spawnManagers = new Dictionary<EnemiesMode, SpawnManager>
        {
            { EnemiesMode.grind, grindSpawnManager },
            { EnemiesMode.waves, wavesSpawnManager }
        };
    }
    private void Start()
    {
        foreach (SpawnManager manager in spawnManagers.Values)
            manager.Init(this);

        spawnManagers[EnemiesMode.waves].enabled = false;
        mode = EnemiesMode.grind;
        spawnManagers[mode].OnEnterMode();
        StartCoroutine(spawnManagers[mode].UpdateEnemiesCo());
    }

    public void ChangeMode()
    {
        spawnManagers[mode].enabled = false;
        spawnManagers[mode].OnExitMode();
        mode = (mode == EnemiesMode.grind) ? EnemiesMode.waves : EnemiesMode.grind;
        spawnManagers[mode].OnEnterMode();
        spawnManagers[mode].enabled = true;
    }

    public Enemy[] GetAllEnemies()
    {
        return enemiesObj.GetComponentsInChildren<Enemy>();
    }

    public EnemyMovement[] GetAllEnemyMovements()
    {
        return enemiesObj.GetComponentsInChildren<EnemyMovement>();
    }

    public void RemoveAllEnemies()
    {
        foreach (Enemy enemy in GetAllEnemies())
            Destroy(enemy.transform.gameObject);
    }

    public Transform GetNearestToCamera(List<EnemyMovement> enemyMovs)
    {
        Vector2 cameraPos = Camera.main.transform.position;
        Vector2 enemyPos = enemyMovs[0].transform.position;

        float minDistance = (enemyPos - cameraPos).magnitude;
        Transform nearest = enemyMovs[0].transform;
        for (int i = 1; i < enemyMovs.Count; i ++)
        {
            enemyPos = enemyMovs[i].transform.position;
            float distance = (enemyPos - cameraPos).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemyMovs[i].transform;
            }
        }
        return nearest;
    }

    public List<EnemyMovement> GetInisibleEnemyMovements()
    {
        List<EnemyMovement> invisibleEnemyMovs = new List<EnemyMovement>();

        foreach (EnemyMovement mov in GetAllEnemyMovements())
            if (!mov.isVisible)
                invisibleEnemyMovs.Add(mov);
        return invisibleEnemyMovs;
    }

    public void UpdateEnemies()
    {
        StartCoroutine(spawnManagers[mode].UpdateEnemiesCo());
    }

    public EnemyTargetAI GetTargetAI()
    {
        return spawnManagers[mode].targetAI;
    }

    public bool IgnoreChaseRadius()
    {
        return mode == EnemiesMode.waves;
    }

    public bool CanUseTent()
    {
        return mode == EnemiesMode.grind;
    }

    public bool CanMakeLoot()
    {
        return mode == EnemiesMode.grind;
    }
}
