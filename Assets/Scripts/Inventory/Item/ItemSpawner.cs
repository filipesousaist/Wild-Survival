using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    public float delay;

    public GameObject obj;

    private float lastSpawned;

    private List<Transform> spawnPoints;

    private void Start()
    {
        spawnPoints = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        //Remove parent transform
        spawnPoints.Remove(transform);

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawned > delay) {
            Spawn();
        }
    }

    private void Spawn() {
        lastSpawned = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(obj, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
