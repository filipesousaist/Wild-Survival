﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Sprite selectPartySprite;
    public Rhino rhino;

    //Equipment list 0-upperBody 1-bottomBody 2-Weapon
    public List<Equipment> equipments;


    public bool HasRhino() {
        return rhino != null;
    }

    public void SetRhino(Rhino rhino) {
        this.rhino = rhino;
    }
    public override void FullRestore()
    {
        base.FullRestore();
        animator.SetBool("dead", false);
    }

    override public void UpdateBarHealth()
    {
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();

        if (manager.IsCurrentActivist(this))
        {
            barHealth.value = Mathf.Max(health, 0);
            healthSignal.Raise();
        }
    }

    //for now just the level increases, don't know what else to do, perhaps damage and hp?
    override public void ReceiveXp(int xpReward)
    {
        base.ReceiveXp(xpReward);
        
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        if (manager.IsCurrentActivist(this))
        {
            barXp.value = xp;
            XpSignal.Raise();
        }
    }

    protected override void UpdateRequiredXp()
    {
        //for now a simple xp curve, can make more complex later
        requiredXp = level * 10;
    }

    override protected void IncreaseAttributes()
    {
        baseAttack += 1;
    }

    override protected void OnDeath()
    {
        if (((PlayerMovement)movement).currentState != PlayerState.dead)
        {
            animator.SetBool("dead", true);
            movement.Die();
            StartCoroutine(DeathCo());
        }
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        manager.activistsDead++;

        if (manager.IsCurrentActivist(this))
            manager.ChangePlayer();
    }

    IEnumerator DeathCo()
    {
        animator.SetBool("firstTimeDying", true);
        yield return null;
        animator.SetBool("firstTimeDying", false);
    }
}
