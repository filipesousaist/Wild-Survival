using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMaterials : MonoBehaviour
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
    }
}
