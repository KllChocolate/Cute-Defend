using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int space = 20;
    public List<Item> items = new List<Item>();
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public void Add(Item item)
    {
        if (item.showIntenvory)
        {
            if (items.Count >= space)
                return;
            items.Add(item);
            if(onItemChangedCallback!=null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

}
