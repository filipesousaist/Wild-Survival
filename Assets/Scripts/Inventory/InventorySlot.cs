using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    //videos
    //4->https://www.youtube.com/watch?v=HQNl3Ff2Lpo
    //5->https://www.youtube.com/watch?v=w6_fetj9PIw
    //6->https://www.youtube.com/watch?v=YLhj7SfaxSE

    Item item;

    public Image icon;

    //public Button removeButton;

    public int index;

    public TMP_Text numberHeld;

    int quantity;

    void Start()
    {
        quantity = 0;
    }
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        //removeButton.interactable = true;
        quantity += 1;
        numberHeld.text = quantity.ToString();
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        //removeButton.interactable = false;
    }

    /*public void OnRemoveButton()
    {
        int index = transform.GetSiblingIndex();
        //Debug.Log("slot index=" + transform.GetSiblingIndex());
        Inventory.instance.Remove(index);
    }*/

    public void UpdateNumber(int number) {
        numberHeld.text = number.ToString();
    }

    public string GetName() {
        return item.name;
    }
    
    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
