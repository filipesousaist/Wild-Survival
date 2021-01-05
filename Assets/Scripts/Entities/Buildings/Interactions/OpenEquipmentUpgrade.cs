using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEquipmentUpgrade : Interactable
{
    public GameObject upgradeUI;

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();
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
