using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Entity
{
    public Player owner;

    override protected void OnAwake() {}

    public override void FullRestore()
    {
        base.FullRestore();
        GetComponent<RhinoMovement>().currentState = RhinoState.command;
    }
    override protected void OnDeath()
    {
        movement.Flee();
    }
}
