using UnityEngine;
using UnityEngine.UI;

public class MaterialSlot : MonoBehaviour
{
    Item item;

    public Image icon;

    public MaterialUI materialUI;

    public Text text;

    public int requiredNumber;
    public void AddItem(Item newItem, int n)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        requiredNumber = n;        
        text.text = n.ToString();
    }

    public string GetName() {
        return item.name;
    }

    public Item GetItem()
    {
        return item;
    }
}
