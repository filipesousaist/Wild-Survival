using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public TMP_Text stat;

    public TMP_Text upgradeText;
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
        currentPlayer.GetComponent<PlayerStats>().UpdateEquipments();
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

        CheckMats();

        if (currentEquipment.armorToUpgrade > 0)
        {
            stat.text = "Armor";
            upgradeText.text = currentEquipment.currentArmor.ToString() + "	---->	      " + (currentEquipment.currentArmor + 1).ToString();
        }
        else
        {
            stat.text = "Damage";
            upgradeText.text = currentEquipment.currentDMG.ToString() + "	---->	      " + (currentEquipment.currentDMG + 1).ToString();
        }
    }

    public void OnUpgradeButton()
    {
        if (!CheckMats()) { return; }

        foreach (MaterialSlot mat in slots)
        {
            var item = mat.GetItem();
            inventory.Remove(item, mat.requiredNumber);
        }

        currentEquipment.Upgrade();

        //UpdatePlayerEquipment();
        UpdateUI(currentEquipment);
    }

    public bool CheckMats()
    {
        bool canUpgrade = true;
        foreach (MaterialSlot mat in slots)
        {
            int requiredNumber = mat.requiredNumber;
            var hasItem = inventory.items.ContainsKey(mat.GetItem());
            if (!hasItem || requiredNumber > inventory.items[mat.GetItem()])
            {
                canUpgrade = false;
                if (hasItem)
                {
                    mat.text.text = inventory.items[mat.GetItem()].ToString() + "/" + requiredNumber.ToString();
                }
                else
                {
                    mat.text.text = "0/" + requiredNumber.ToString();
                }
            }
            else
            {
                mat.text.text = inventory.items[mat.GetItem()].ToString() + "/" + requiredNumber.ToString();
            }
        }

        if (!canUpgrade)
        {
            Debug.Log("Not enough materials");
            var temp = upgrade.image.color;
            temp.r = 0.7843137f;
            temp.g = 0.7843137f;
            temp.b = 0.7843137f;
            upgrade.image.color = temp;
            return false;
        }
        var tmp = upgrade.image.color;
        tmp.r = 1;
        tmp.g = 1;
        tmp.b = 1;
        upgrade.image.color = tmp;
        return true;
    }
}
