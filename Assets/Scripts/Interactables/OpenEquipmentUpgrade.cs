using System.Collections;
using UnityEngine;

public class OpenEquipmentUpgrade : Interactable
{
    private GameObject upgradeUIGameobject;
    private MaterialUI materialUI;

    protected override void OnAwake()
    {
        base.OnAwake();
        upgradeUIGameobject = FindObjectOfType<Canvas>().transform.Find("ItemsUpgradeMenu").gameObject;
        materialUI = FindObjectOfType<MaterialUI>();
    }

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();
        ToggleUI();
    }

    private void ToggleUI() {
        if (!upgradeUIGameobject.activeSelf)
        {
            materialUI.CheckMats();
        }
        upgradeUIGameobject.SetActive(!upgradeUIGameobject.activeSelf);
    }

    protected override void OnPlayerMoveAway()
    {
        if (upgradeUIGameobject.activeSelf) {
            ToggleUI();
        }
    }

    public override string GetInteractText()
    {
        return upgradeUIGameobject.activeSelf ? "Close" : "Use";
    }
}
