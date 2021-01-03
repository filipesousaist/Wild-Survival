using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //videos
    //4->https://www.youtube.com/watch?v=HQNl3Ff2Lpo
    //5->https://www.youtube.com/watch?v=w6_fetj9PIw
    //6->https://www.youtube.com/watch?v=YLhj7SfaxSE

    Inventory inventory;

    public Transform itemsParent;

    public InventorySlot itemSlotPrefab;

    [SerializeField] List<InventorySlot> slots;

    public GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = new List<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI() 
    {
        //https://www.youtube.com/watch?v=QPYGPGvg0PI
        if (inventory.items.Count > slots.Count) {
            InventorySlot slot = Instantiate(itemSlotPrefab);
            slot.transform.SetParent(itemsParent, false);
            slot.AddItem(inventory.items[inventory.items.Count - 1]);
            slots.Add(slot);
        }
        else if(inventory.items.Count < slots.Count) {
            var slot = slots[inventory.GetLastRemovedIndex()];
            slots.RemoveAt(inventory.GetLastRemovedIndex());
            Destroy(slot.gameObject);
        }
        /*for (int i = 0; i <= slots.Count; i++) 
        {
            if (i < inventory.items.Count) 
            {
                slots[i].AddItem(inventory.items[i]);
                /*InventorySlot slot = Instantiate(itemSlotPrefab);
                slot.index = slots.Count;
                slot.transform.parent = itemsParent;
                slot.transform.localScale = slots[0].transform.localScale;
                slot.AddItem(inventory.items[i]);
                slots.Add(slot);*/
           // }
           // else 
           // {
           //     slots[i].ClearSlot();
               /* var lastSlot = slots[i];
                slots.RemoveAt(i);
                Destroy(lastSlot.gameObject);*/

        //    }
       // }
       /* if (inventory.items.Count >= inventory.space)
        {
            if (inventory.items.Count > slots.Count) 
            {
                InventorySlot slot = Instantiate(itemSlotPrefab);
                slot.index = slots.Count;
                slot.transform.parent = itemsParent;
                slot.transform.localScale = slots[0].transform.localScale;
                slot.AddItem(inventory.items[inventory.items.Count - 1]);
                slots.Add(slot);
            }
            if (inventory.items.Count < slots.Count) 
            {
                //var index = inventory.GetLastRemovedIndex();
                var lastSlot = slots[index];
                slots.RemoveAt(index);
                Destroy(lastSlot.gameObject);
            }
        }*/
    }
}
