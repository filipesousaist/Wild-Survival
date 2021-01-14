using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControls : MonoBehaviour
{
    public Item wood;
    public Item leather;
    public Item rock;

    private readonly int num = 50;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            for (int i = 0; i < 50; i++)
            {
                Inventory.instance.Add(wood);
                Inventory.instance.Add(leather);
                Inventory.instance.Add(rock);
            }
            Debug.Log("Added to inventory " + num + " of each material.");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (Building building in FindObjectsOfType<Building>())
            {
                if (building.level == 0) building.Upgrade();
                else building.Repair();
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
            foreach (Building building in FindObjectsOfType<Building>())
                building.TakeDamage(building.maxHealth);
    }
}
