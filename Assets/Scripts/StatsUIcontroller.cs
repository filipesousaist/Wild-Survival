using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUIcontroller : MonoBehaviour
{
    public StatsUI statsUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            statsUI.gameObject.SetActive(!statsUI.gameObject.activeSelf);
            if (statsUI.gameObject.activeSelf)
            {
                statsUI.UpdateUI();
            }
        }
    }
}
