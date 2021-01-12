using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    Inventory inventory;

    public Transform materials;

    public MaterialSlot materialSlotPrefab;

    Dictionary<string, int> matsCount;

    [SerializeField] List<MaterialSlot> slots;

    public ActivistsManager activistsManager;

    public Text title;

    public Button button;
    public Text buttonText;

    private GameObject buildUIObject;

    private Building currentBuilding;

    private void Awake()
    {
        Transform buildMenu = FindObjectOfType<Canvas>().transform.Find("BuildMenu");
        buildUIObject = buildMenu.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        slots = new List<MaterialSlot>();
        matsCount = new Dictionary<string, int>();
    }

    public void UpdateUI(Building building)
    {
        currentBuilding = building;
        int incr = building.level == 0 ? 1 : 0;

        List<MatDict> mats = building.materials;
        for (int i = 0; i < slots.Count && i < mats.Count; i++)
        {
            slots[i].AddItem(mats[i].item, mats[i].n * (building.level + incr));
            if (!matsCount.ContainsKey(mats[i].item.name))
            {
                matsCount.Add(mats[i].item.name, 0);
            }
        }
        if (slots.Count < mats.Count)
        {
            Debug.Log("Slots: " + slots.Count);
            Debug.Log("Mats: " + mats.Count);
            for (int i = slots.Count; i < mats.Count; i++)
            {
                MaterialSlot slot = Instantiate(materialSlotPrefab);
                slot.transform.parent = materials;
                slot.transform.localScale = new Vector3(1, 1, 1);
                slot.AddItem(mats[i].item, mats[i].n * (building.level + incr));
                slots.Add(slot);
                if (!matsCount.ContainsKey(mats[i].item.name))
                {
                    matsCount.Add(mats[i].item.name, 0);
                }
            }
        }
        if (slots.Count > mats.Count)
        {
            for (int i = mats.Count; i < slots.Count; i++)
            {
                var slot = slots[i];
                slots.RemoveAt(i);
                matsCount.Remove(slot.GetName());
                Destroy(slot.gameObject);
            }
        }

        string action = building.level == 0 ? "Build" : "Repair";
        title.text = action + " " + building.entityName;
        buttonText.text = action;
    }

    public void OnBuildButton()
    {
        //Dictionary<string, int> tempMatsCount = new Dictionary<string, int>(matsCount);
        /*foreach (Item item in inventory.items)
        {
            if (tempMatsCount.ContainsKey(item.name))
            {
                tempMatsCount[item.name] ++;
            }
            else
                tempMatsCount[item.name] = 0;
        }*/
        foreach (MaterialSlot mat in slots)
        {
            //string name = mat.GetName();
            int requiredNumber = int.Parse(mat.requiredNumber.text);
            if (requiredNumber > inventory.items[mat.GetItem()])
            {
                Debug.Log("Not enough materials");
                return;
            }
            //tempMatsCount[name] = requiredNumber;
        }

        foreach (MaterialSlot mat in slots)
        {
            var item = mat.GetItem();
            inventory.Remove(item, int.Parse(mat.requiredNumber.text));
        }

        if (currentBuilding.level == 0)
            currentBuilding.Upgrade();
        else
            currentBuilding.Repair();

        buildUIObject.SetActive(false);
    }
}
