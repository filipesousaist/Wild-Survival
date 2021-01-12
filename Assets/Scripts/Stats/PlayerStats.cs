using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private List<Equipment> equipments;


    public void UpdateEquipments()
    {
        equipments = gameObject.GetComponent<Player>().equipments;
        armor.ClearModifier();
        damage.ClearModifier();
        foreach(Equipment equipment in equipments)
        {
            armor.AddModifier(equipment.currentArmor);
            damage.AddModifier(equipment.currentDMG);
        }
    }
}
