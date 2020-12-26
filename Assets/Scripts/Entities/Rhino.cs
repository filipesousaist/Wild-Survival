using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Entity
{
    public Player owner;
    public int level;
    public int xp;
    public int requiredXp;
    override protected void OnAwake() 
    {
        level = 1;
        requiredXp = level * 10;
    }

    public override void FullRestore()
    {
        base.FullRestore();
        GetComponent<RhinoMovement>().currentState = RhinoState.walk;
    }
    //for now just the level increases, don't know what else to do, perhaps damage and hp?
    public void ReceiveXp(int xpReward)
    {
        //for now a simple xp curve, can make more complex later
        xp += xpReward;
        while (xp > requiredXp)
        {
            xp -= requiredXp;
            level++;
            requiredXp = level * 10;
        }
    }
    override protected void OnDeath()
    {
        movement.Flee();
    }
}
