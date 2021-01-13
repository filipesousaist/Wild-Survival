using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEquipmentMission : Mission
{
    public int numUpgrades;

    public ActivistsManager activistsManager;

    private int GetTotalNumUpgrades()
    {
        int total = 0;
        foreach (Player activist in activistsManager.players)
            foreach (Equipment equipment in activist.equipments)
                total += equipment.level - equipment.initialLevel;
        return total;
    }

    public override bool IsCompleted()
    {
        return GetTotalNumUpgrades() >= numUpgrades;
    }

    public override string GetMessage()
    {
        int num = GetTotalNumUpgrades();
        string s = numUpgrades == 1 ? "" : "s";
        return "Make " + numUpgrades + " upgrade" + s + " at the upgrade table: " + num + "/" + numUpgrades;
    }
}
