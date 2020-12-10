using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Entity
{
    public Player owner;
    protected override void OnDeath()
    {
        movement.Flee();
    }
}
