using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : InteractiveObject
{
    public Item item;
    public override void Interact()
    {
        base.Interact();
        pickUp();
    }
    void pickUp()
    { 
        Inventory.instance.Add(item);
        Destroy(gameObject);
    }
}
