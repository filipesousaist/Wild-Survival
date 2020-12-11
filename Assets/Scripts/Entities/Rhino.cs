using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Entity
{
    public Player owner;

    override protected void OnAwake() {}
    override protected void OnDeath()
    {
        movement.Flee();
    }
}
