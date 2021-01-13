public class CollectFoodMission : Mission
{
    public int amount;
    public FoodItem[] foodItems;
    private int CountFood()
    {
        int count = 0;
        foreach (FoodItem item in foodItems)
            count += Inventory.instance.Count(item);
        return count;
    }

    public override bool IsCompleted()
    {
        return CountFood() >= amount;
    }

    public override string GetMessage()
    {
        return "Collect food: " + CountFood() + "/" + amount;
    }


}
