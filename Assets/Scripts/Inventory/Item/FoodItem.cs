using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/FoodItem")]
public class FoodItem : Item
{
    public FloatValue currentFood;
    public Signal foodSignal; 
    public override void Use() {
        currentFood.value = Mathf.Min(currentFood.value + 1, Food.MAX);
        foodSignal.Raise();
        base.Use();
    }
}
