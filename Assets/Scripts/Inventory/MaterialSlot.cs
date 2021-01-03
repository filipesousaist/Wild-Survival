using UnityEngine;
using UnityEngine.UI;

public class MaterialSlot : MonoBehaviour
{
    Item item;

    public Image icon;

    public MaterialUI materialUI;

    public Text requiredNumber;

    public void AddItem(Item newItem, int n)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;

        requiredNumber.text = n.ToString();
    }
}
