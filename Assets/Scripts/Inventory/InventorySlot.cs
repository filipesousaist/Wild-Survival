using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //videos
    //4->https://www.youtube.com/watch?v=HQNl3Ff2Lpo
    //5->https://www.youtube.com/watch?v=w6_fetj9PIw
    //6->https://www.youtube.com/watch?v=YLhj7SfaxSE

    Item item;

    public Image icon;

    public Button removeButton;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }
    
    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
