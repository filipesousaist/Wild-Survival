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

    public GameObject helpArrow;
    private static readonly Vector3 E_Y = new Vector3(0, 1, 0);
    private static readonly Vector3 E_Z = new Vector3(0, 0, 1);
    private static readonly float QUARTER_H = Screen.height / 4;

    private void Awake()
    {
        enemiesObj = GameObject.Find("Enemies");
    }
    private void Start()
    {
        spawnManagers = new Dictionary<EnemiesMode, SpawnManager>();
        spawnManagers.Add(EnemiesMode.grind, grindSpawnManager);
        spawnManagers.Add(EnemiesMode.waves, wavesSpawnManager);
        foreach (SpawnManager manager in spawnManagers.Values)
            manager.Init(this);

        mode = EnemiesMode.grind;
        spawnManagers[mode].OnEnterMode();
        StartCoroutine(spawnManagers[mode].UpdateAll());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            ChangeMode();
    }

    private void FixedUpdate()
    {
        if (mode == EnemiesMode.grind)
            spawnManagers[mode].UpdateText();
        else // waves
            UpdateHelpArrow();
    }

    private void ChangeMode()
    {
        spawnManagers[mode].OnExitMode();
        mode = (mode == EnemiesMode.grind) ? EnemiesMode.waves : EnemiesMode.grind;
        spawnManagers[mode].OnEnterMode();
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

    private void UpdateHelpArrow()
    {
        List<EnemyMovement> invisibleEnemyMovs = GetInisibleEnemyMovements();

        int numInvisble = invisibleEnemyMovs.Count;
        if (numInvisble > 0)
        {
            Vector2 difference = GetNearestToCamera(invisibleEnemyMovs).position - Camera.main.transform.position;

            float angle = Vector3.SignedAngle(E_Y, difference.normalized, E_Z);
            helpArrow.transform.eulerAngles = 
                new Vector3(0, 0, angle);
            helpArrow.transform.localPosition = new Vector3(
                -QUARTER_H * Mathf.Sin(angle * Mathf.Deg2Rad),
                QUARTER_H * Mathf.Cos(angle * Mathf.Deg2Rad), 
                0);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);

    }

    private Transform GetNearestToCamera(List<EnemyMovement> enemyMovs)
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

    private List<EnemyMovement> GetInisibleEnemyMovements()
    {
        List<EnemyMovement> invisibleEnemyMovs = new List<EnemyMovement>();

        foreach (EnemyMovement mov in GetAllEnemyMovements())
            if (!mov.isVisible)
                invisibleEnemyMovs.Add(mov);
        return invisibleEnemyMovs;
    }

    public void UpdateBar()
    {
        StartCoroutine(spawnManagers[mode].UpdateAll());
    }
}
