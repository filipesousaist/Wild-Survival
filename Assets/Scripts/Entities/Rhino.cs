using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Character
{
    private static readonly int XP_MULT = 15;
    private static readonly int RADIATION_REQUIREMENT = 10;
    public Player owner;

    [ReadOnly] public int radiation;

    override protected void OnAwake() 
    {
        base.OnAwake();
        radiation = 0;
        requiredXp = level * XP_MULT;
    }

    public override void FullRestore()
    {
        base.FullRestore();
        GetComponent<RhinoMovement>().currentState = RhinoState.walk;
    }

    override public void UpdateBarHealth()
    {
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();

        if (manager.IsCurrentActivist(owner))
        {
            barHealth.value = Mathf.Max(health, 0);
            healthSignal.Raise();
        }
    }
    public void ReceiveRadiation(int radiationReceived)
    {
        radiation += radiationReceived;
        if (radiation >= RADIATION_REQUIREMENT)
        {
            radiation = 0;
            GetMutation();
        }

    }

    override public void ReceiveXp(int xpReward)
    {
        base.ReceiveXp(xpReward);

        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        if (manager.IsCurrentActivist(owner))
        {
            barXp.value = xp;
            XpSignal.Raise();
        }
    }

    private void GetMutation()
    {

    }

    protected override void UpdateRequiredXp()
    {
        //for now a simple xp curve, can make more complex later
        requiredXp = level * 15;
    }

    override protected void IncreaseAttributes()
    {
        baseAttack += 1.5f;
    }

    override protected void OnDeath()
    {
        movement.Flee();
    }
}
