using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUI : MonoBehaviour
{
    Inventory inventory;

    public Transform equipments;

    public Transform materials;

    public MaterialSlot materialSlotPrefab;

    [SerializeField] List<MaterialSlot> slots;

    //[SerializeField] List<EquipmentSlot> EquipmentSlots;

    public ActivistsManager activistsManager;

    public Button upgrade;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        slots = new List<MaterialSlot>();
        activistsManager.onPlayerChangedCallback += UpdatePlayerEquipment;

    }

    void UpdatePlayerEquipment() {
        var currentPlayer = activistsManager.GetCurrentPlayer();
        for (int i = 0; i < equipments.childCount; i++)
        {
            EquipmentSlot slot = equipments.transform.GetChild(i).GetComponent<EquipmentSlot>();
            slot.AddItem(currentPlayer.equipments[i]);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i].gameObject);
        }
        slots.Clear();
    }

    public void UpdateUI(Equipment equipment) {
        List<MatDict> mats = equipment.materials;
        for (int i = 0; i < slots.Count; i++)
        {
            if(i >= mats.Count) {
                break;
            }
            slots[i].AddItem(mats[i].item, mats[i].n);
        }
        if(slots.Count < mats.Count) {
            for (int i = slots.Count; i < mats.Count; i++)
            {
                MaterialSlot slot = Instantiate(materialSlotPrefab);
                slot.transform.parent = materials;
                slot.transform.localScale = new Vector3(1, 1, 1);
                slot.AddItem(mats[i].item, mats[i].n);
                slots.Add(slot);
            }
        }
        if(slots.Count > mats.Count)
        {
            for (int i = mats.Count; i < slots.Count; i++)
            {
                var slot = slots[i];
                slots.RemoveAt(i);
                Destroy(slot.gameObject);
            }
        }
    }
}
