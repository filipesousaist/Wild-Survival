using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrindSpawnManager : SpawnManager
{
    public int maxEnemies;
    public Vector2[] spawnPoints;
    public float minSpawnDistance;

    public string welcomeMessage;
    public string[] miscMessages;

    public float welcomeMessageDuration;
    public float miscMessageDuration;

    public float spawnPeriod;
    private float spawnTime;

    private void FixedUpdate()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnPeriod)
        {
            spawnTime = 0;
            UpdateEnemies();
        }
    }

    override public void OnEnterMode()
    {
        StartCoroutine(UpdateAll());
        spawnTime = 0;
    }

    override public void OnExitMode()
    {
        enemiesManager.RemoveAllEnemies();
    }

    override protected void UpdateEnemies()
    {
        Enemy[] enemies = enemiesManager.GetAllEnemies();
        if (enemies.Length < maxEnemies)
            SpawnEnemy(GetRandomSpawnPoint());
    }


    private Vector2 GetRandomSpawnPoint()
    {
        foreach (List<Vector2> possibleSpawnPoints in GetPossibleSpawnPoints())
        {
            int count = possibleSpawnPoints.Count;
            if (count > 0)
                return possibleSpawnPoints[Random.Range(0, count)];
        }
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private List<Vector2>[] GetPossibleSpawnPoints()
    {
        List<Vector2> farFromCurrentPlayer = new List<Vector2>();
        List<Vector2> alsoFarFromActivists = new List<Vector2>();
        List<Vector2> alsoFarFromRhinos = new List<Vector2>();
        List<Vector2> alsoFarFromEnemies = new List<Vector2>();

        foreach (Vector2 point in spawnPoints)
        {
            if (!IsFarFromCurrentPlayer(point))
                continue;
            if (!IsFarFromAllActivists(point))
            {
                farFromCurrentPlayer.Add(point);
                continue;
            }
            if (!IsFarFromAllRhinos(point))
            {
                alsoFarFromActivists.Add(point);
                continue;
            }
            if (!IsFarFromAllEnemies(point)) {
                alsoFarFromRhinos.Add(point);
                continue;
            }
            alsoFarFromEnemies.Add(point);
        }

        return new List<Vector2>[] {alsoFarFromEnemies, alsoFarFromRhinos, alsoFarFromActivists, farFromCurrentPlayer};
    }

    private bool IsFar(EntityMovement mov, Vector3 point)
    {
        return (mov.transform.position - point).magnitude > minSpawnDistance;
    }

    private bool IsFarFromCurrentPlayer(Vector3 point)
    {
        PlayerMovement playerMov = FindObjectOfType<ActivistsManager>().GetCurrentPlayerMovement();
        return IsFar(playerMov, point);
    }

    private bool IsFarFromAll(EntityMovement[] entityMovements, Vector3 point)
    {
        foreach (EntityMovement mov in entityMovements)
            if (!IsFar(mov, point))
                return false;
        return true;
    }

    private bool IsFarFromAllActivists(Vector3 point)
    {
        return IsFarFromAll(FindObjectsOfType<PlayerMovement>(), point);
    }

    private bool IsFarFromAllRhinos(Vector3 point)
    {
        return IsFarFromAll(FindObjectsOfType<RhinoMovement>(), point);
    }

    private bool IsFarFromAllEnemies(Vector3 point)
    {
        return IsFarFromAll(FindObjectsOfType<EnemyMovement>(), point);
    }

    override public void UpdateText()
    {
        string newText;
        if (Time.time <= welcomeMessageDuration)
            newText = welcomeMessage;
        else
        {
            int messageIndex = Mathf.FloorToInt((Time.time - welcomeMessageDuration) / miscMessageDuration);
            newText = miscMessages[messageIndex % miscMessages.Length];
        }

        textObj.GetComponent<Text>().text = newText;
    }
}
