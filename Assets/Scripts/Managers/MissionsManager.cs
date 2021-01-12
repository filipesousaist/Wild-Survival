using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    public Mission[] missions;
    private int current;
    private int numMissions;

    public GameObject missionTextObject;
    private Text missionText;

    private void Awake()
    {
        missionText = missionTextObject.GetComponent<Text>();
    }

    private void Start()
    {
        current = 0;
        numMissions = missions.Length;
        missions[current].OnBegin();
    }

    private void Update()
    {
        if (missions[current].IsCompleted())
        {
            missions[current].OnFinish();
            current ++;
            if (current < numMissions)
                missions[current].OnBegin();
        }
        missionText.text = current < numMissions ? missions[current].GetMessage()
                                                 : "All missions completed! :)";
    }
}
