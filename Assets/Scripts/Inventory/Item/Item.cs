using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item";

    public Sprite icon = null;

    public bool usable;

    public bool isDefaultItem = false;

    [SerializeField] int numberHeld;

    public virtual void Use() {
        Debug.Log("Using" + name);
        if (usable)
        {
            RemoveFromInventory();
        }
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
