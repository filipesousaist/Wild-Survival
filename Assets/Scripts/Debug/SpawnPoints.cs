using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private float minFov = 15f;
    private float maxFov = 90f;
    private float sensitivity = 10f;

    private float camSpeed = 15f;

    public GameObject prefab;

    private void Start()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);

        foreach (Vector2 point in FindObjectOfType<GrindSpawnManager>().spawnPoints)
        {
            Instantiate(prefab);
            prefab.transform.position = point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Time.timeScale = 1 - Time.timeScale;

        float ortSize = Camera.main.orthographicSize;
        ortSize -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        ortSize = Mathf.Clamp(ortSize, minFov, maxFov);
        Camera.main.orthographicSize = ortSize;
        Camera.main.transform.position = new Vector3(0, 0, -10);

        Camera.main.transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * camSpeed;
    }
}
