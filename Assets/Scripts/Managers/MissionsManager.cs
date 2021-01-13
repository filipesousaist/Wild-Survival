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
    private static readonly float MSG_SHAKE_SPEED = 10;

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
        if (timeForNextMission > 0)
        {
            UpdateFinishMessage(Time.time);
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

        missions[current].UpdateHelpArrow();
    }

    private void BeginMission()
    {
        if ((++current) < numMissions)
            StartCoroutine(missions[current].Begin());
    }

    private void FinishMission()
    {
        StartCoroutine(missions[current].Finish());
        timeForNextMission = 5;
    }

    private void UpdateMessage()
    {
        missionText.transform.localPosition = Vector3.zero;
        missionText.text = current < numMissions ? missions[current].GetMessage()
                                         : "All missions completed! :)";
    }

    private void UpdateFinishMessage(float time)
    {
        string finishMessage = missions[current].GetFinishMessage();
        missionText.text = finishMessage ?? "Task completed!";
        missionText.transform.localPosition = MSG_SHAKE_SPEED * (Mathf.PingPong(time, 1) * 2 - 1) * Vector3.right;
    }

    public Mission GetCurrentMission()
    {
        return missions[current];
    }
}
