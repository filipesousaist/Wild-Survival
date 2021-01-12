using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    private Mission[] missions;
    private int current;
    private int numMissions;

    public GameObject missionsObject;
    public GameObject missionTextObject;
    private Text missionText;

    public GameObject helpArrow;

    private void Awake()
    {
        missionText = missionTextObject.GetComponent<Text>();
        missions = missionsObject.GetComponentsInChildren<Mission>();
        foreach (Mission m in missions)
            m.gameObject.SetActive(false);
    }

    private void Start()
    {
        current = 0;
        numMissions = missions.Length;
        StartCoroutine(missions[current].Begin());
    }

    private void Update()
    {
        missions[current].UpdateHelpArrow(helpArrow);
        if (missions[current].IsCompleted())
        {
            StartCoroutine(missions[current].Finish());
            current ++;
            if (current < numMissions)
                StartCoroutine(missions[current].Begin());
        }
        missionText.text = current < numMissions ? missions[current].GetMessage()
                                                 : "All missions completed! :)";
    }
}
