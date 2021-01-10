using System.Collections;
using UnityEngine;

public class OpenEquipmentUpgrade : Interactable
{
    public GameObject upgradeUI;

    protected override void OnAwake()
    {
        base.OnAwake();
        upgradeUI = FindObjectOfType<Canvas>().transform.Find("ItemsUpgradeMenu").gameObject;
    }

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();
        ToggleUI();
    }

    private void ToggleUI() {
        upgradeUI.SetActive(!upgradeUI.activeSelf);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (upgradeUI.activeSelf) {
            ToggleUI();
        }
    }
}
