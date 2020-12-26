using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //videos
    //4->https://www.youtube.com/watch?v=HQNl3Ff2Lpo
    //5->https://www.youtube.com/watch?v=w6_fetj9PIw
    //6->https://www.youtube.com/watch?v=YLhj7SfaxSE

    Inventory inventory;

    public Transform itemsParent;

    InventorySlot[] slots;

    public GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
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
        for (int i = 0; i < slots.Length; i++) 
        {
            if (i < inventory.items.Count) 
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else 
            {
                slots[i].ClearSlot();
            }
        }
    }
}
