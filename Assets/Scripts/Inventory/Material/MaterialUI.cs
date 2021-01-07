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
    
    Equipment currentEquipment;

    Dictionary<string, int> matsCount;

    [SerializeField] List<MaterialSlot> slots;

    public ActivistsManager activistsManager;

    public Button upgrade;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        slots = new List<MaterialSlot>();
        activistsManager.onPlayerChangedCallback += UpdatePlayerEquipment;
        matsCount = new Dictionary<string, int>();
        UpdatePlayerEquipment();
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
        matsCount.Clear();
    }

    public void UpdateUI(Equipment equipment) {
        currentEquipment = equipment;
        List<MatDict> mats = equipment.materials;
        for (int i = 0; i < slots.Count; i++)
        {
            if(i >= mats.Count) {
                break;
            }
            slots[i].AddItem(mats[i].item, mats[i].n * equipment.level);
            if (!matsCount.ContainsKey(mats[i].item.name)) { 
                matsCount.Add(mats[i].item.name, 0);
            }
        }
        if(slots.Count < mats.Count) {
            for (int i = slots.Count; i < mats.Count; i++)
            {
                MaterialSlot slot = Instantiate(materialSlotPrefab);
                slot.transform.parent = materials;
                slot.transform.localScale = new Vector3(1, 1, 1);
                slot.AddItem(mats[i].item, mats[i].n * equipment.level);
                slots.Add(slot);
                if (!matsCount.ContainsKey(mats[i].item.name))
                {
                    matsCount.Add(mats[i].item.name, 0);
                }
            }
        }
        if(slots.Count > mats.Count)
        {
            for (int i = mats.Count; i < slots.Count; i++)
            {
                var slot = slots[i];
                slots.RemoveAt(i);
                matsCount.Remove(slot.GetName());
                Destroy(slot.gameObject);
            }
        }
    }

    public void OnUpgradeButton()
    {
        Dictionary<string, int> tempMatsCount = new Dictionary<string, int>(matsCount);
        foreach (var item in inventory.items)
        {
            if (tempMatsCount.ContainsKey(item.name)) {
                tempMatsCount[item.name] += 1; 
            }
        }
        foreach (var item in slots)
        {
            if (!item.requiredNumber.text.Equals(tempMatsCount[item.GetName()].ToString()))
            {
                Debug.Log("Not enough materials");
                return;
            }
        }

        currentEquipment.Upgrade();
        foreach (var item in slots)
        {
            var name = item.GetName();
            while (tempMatsCount[name] > 0) {
                inventory.Remove(inventory.items.IndexOf(item.GetItem()));
                tempMatsCount[name] -= 1;
            }
        }
       
        //UpdatePlayerEquipment();
        UpdateUI(currentEquipment);
    }
}
