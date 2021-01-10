using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    override protected void OnStart()
    {
        once = true;
    }

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();

        PickUp();
    }

    void PickUp() {
        Debug.Log("Picking up" + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
