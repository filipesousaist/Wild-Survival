using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/FoodItem")]
public class FoodItem : Item
{
    public FloatValue currentFood;
    public Signal foodSignal; 
    public override void Use() {

        currentFood.value++;
        if(currentFood.value>5)
            currentFood.value=5;
        foodSignal.Raise();
        RemoveFromInventory();
    }
}
