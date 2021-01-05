using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEquipmentUpgrade : Interactable
{
    public GameObject upgradeUI;

    public override void Interact()
    {
        base.Interact();
        OpenUI();
    }

    void OpenUI() {
        upgradeUI.SetActive(!upgradeUI.activeSelf);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (upgradeUI.activeSelf) {
            OpenUI();
        }
    }
}
