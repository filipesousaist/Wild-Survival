﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MatDict
{
    public Item item;
    public int n;
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item, ISerializationCallbackReceiver
{
    //7->https://www.youtube.com/watch?v=d9oLS5hy0zU

    public float armorModifier;
    public float damageModifier;

    public float armorToUpgrade;
    public float damageToUpgrade;

    public int initialLevel;

    [ReadOnly] public int level;

    public List<MatDict> materials;

    public void OnAfterDeserialize()
    {
        level = initialLevel;
    }

    public void OnBeforeSerialize() { }

    public void Upgrade() {
        level += 1;
        UpgradeModifiers();
    }

    private void UpgradeModifiers()
    {
        armorModifier += armorToUpgrade;
        damageToUpgrade += damageToUpgrade;
    }
}
