using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    /*
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }
    */
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
