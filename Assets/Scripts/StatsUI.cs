﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{

    public ActivistsManager activists;

    public Image activistFace;
    public TMP_Text activistName;
    public TMP_Text activistLevel;
    public TMP_Text activistMaxHP;
    public TMP_Text activistDamage;
    public TMP_Text activistArmor;
    public Image rhinoFace;
    public TMP_Text rhinoName;
    public TMP_Text rhinoLevel;
    public TMP_Text rhinoMaxHP;
    public TMP_Text rhinoDamage;

    private void Start()
    {
        activists.onPlayerChangedCallback += UpdateUI;
    }

    public void UpdateUI() { 
        var currentPlayer = activists.GetCurrentPlayer();
        activistFace.sprite = currentPlayer.portrait;
        activistName.text = currentPlayer.name;
        activistLevel.text = currentPlayer.level.ToString();
        activistMaxHP.text = currentPlayer.maxHealth.ToString();
        activistDamage.text = currentPlayer.stats.damage.GetValue().ToString();
        activistArmor.text = currentPlayer.stats.armor.GetValue().ToString();
        var currentRhino = currentPlayer.rhino;
        rhinoFace.sprite = currentRhino.portrait;
        rhinoName.text = currentRhino.name;
        rhinoLevel.text = currentRhino.level.ToString();
        rhinoMaxHP.text = currentRhino.maxHealth.ToString();
        rhinoDamage.text = currentRhino.stats.damage.GetValue().ToString();
        
    }
}
