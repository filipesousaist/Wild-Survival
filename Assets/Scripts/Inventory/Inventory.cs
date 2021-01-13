using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than 1 instance of inventory found!");
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    private int lastItemRemoved;

    public InventoryUI invUI;

    private void Start()
    {
        invUI = FindObjectOfType<InventoryUI>();
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.ContainsKey(item))
            {
                items[item] += 1;
                invUI.UpdateSlot(item, items[item]);
            }
            else { 
                items.Add(item, 1);
                /*if (onItemChangedCallback != null) { 
                    onItemChangedCallback.Invoke();
                }*/
                invUI.AddSlot(item);
            }

        }
        return true;
    }

    /*public void Remove(int index)
    {
        items.RemoveAt(index);
        lastItemRemoved = index;
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }*/

    public void Remove(Item item)
    {
        if (items[item] > 1)
        {
            items[item] -= 1;
            invUI.UpdateSlot(item, items[item]);
        }
        else { 
            items.Remove(item);
            invUI.RemoveSlot(item);
            /*if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }*/
        }

    }


    public void Remove(Item item, int number) {
        if (items[item] - number > 0)
        {
            items[item] -= number;
            invUI.UpdateSlot(item, items[item]);
        }
        else
        {
            items.Remove(item);
            invUI.RemoveSlot(item);
            /*if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }*/
        }
    }

    public int GetLastRemovedIndex() {
        return lastItemRemoved;
    }

    public int Count(Item item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }
}
