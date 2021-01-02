using UnityEngine;

[System.Serializable]
public class Loot {
    public GameObject item;
    public int chance;
}
[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public GameObject LootItem() {
        int cumProb = 0;
        int currentProb = Random.Range(0, 100);
        for (int i = 0; i < loots.Length; i++)
        {
            cumProb += loots[i].chance;
            if (currentProb <= cumProb)
            {
                return loots[i].item;
            }
        }
        return null;
    }
}
