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

    public List<Item> items = new List<Item>();

    //public int space = 9;

    private int lastItemRemoved;
    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            items.Add(item);

            if (onItemChangedCallback != null) { 
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove(int index)
    {
        items.RemoveAt(index);
        lastItemRemoved = index;
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public int GetLastRemovedIndex() {
        return lastItemRemoved;
    }
}
