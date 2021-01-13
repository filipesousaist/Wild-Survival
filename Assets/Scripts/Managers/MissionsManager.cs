using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    private Mission[] missions;
    private int current;
    private int numMissions;

    private float timeForNextMission;

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
        timeForNextMission = 0;
        StartCoroutine(missions[current].Begin());
    }

    private void Update()
    {
        missions[current].UpdateHelpArrow();
        if (timeForNextMission > 0)
        {
            UpdateFinishMessage();
            timeForNextMission -= Time.deltaTime;
            if (timeForNextMission <= 0)
                BeginMission();
        }
        else
        {
            UpdateMessage();
            if (missions[current].IsCompleted())
                FinishMission();
        }
    }

    private void BeginMission()
    {
        if ((++current) < numMissions)
            StartCoroutine(missions[current].Begin());
    }

    private void FinishMission()
    {
        StartCoroutine(missions[current].Finish());
        timeForNextMission = 3;
    }

    private void UpdateMessage()
    {
        missionText.text = current < numMissions ? missions[current].GetMessage()
                                         : "All missions completed! :)";
    }

    private void UpdateFinishMessage()
    {
        string finishMessage = missions[current].GetFinishMessage();
        missionText.text = finishMessage == null ? "Task completed!"
                                                 : finishMessage;
    }

    public Mission GetCurrentMission()
    {
        return missions[current];
    }
}
