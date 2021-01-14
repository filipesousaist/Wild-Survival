using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot   : MonoBehaviour
{
    Equipment item;

    public Image icon;

    public MaterialUI materialUI;

    public Button button;

    public void AddItem(Equipment newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            materialUI.UpdateUI(item);
        }
    }

    public void HighlightButton() {
        foreach (var slot in materialUI.equipSlots)
        {
            slot.DeHighlightButton();
        }
        button.image.color = button.colors.pressedColor;
    }

    public void DeHighlightButton()
    {
        button.image.color = Color.white;
    }
}
