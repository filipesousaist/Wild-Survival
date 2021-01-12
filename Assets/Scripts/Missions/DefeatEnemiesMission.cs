using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemiesMission : Mission
{
    public int amount;

    private int defeated;

    private void Start()
    {
        defeated = 0;
    }

    // Call from SignalListener
    public void OnEnemyDefeated()
    {
        defeated = Mathf.Min(defeated + 1, amount);
    }

    public override bool IsCompleted()
    {
        return defeated == amount;
    }

    public override string ToString()
    {
        return "Defeat zombies: " + defeated + "/" + amount;
    }
}
