using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item" , menuName = "Inventory/Item")] 
public class Item : ScriptableObject 
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool showIntenvory = true;
    

    public void Use()
    {
        if (name.Equals("Potion"))
        {
            PlayerStatus.instance.healing();
            RemoveItemformInventory();
        }
        else if(name.Equals("Axe"))
        {
            PlayerMovement.instance.ShowAxe();  
        }
        else if (name.Equals("Sword 1"))
        {
            PlayerMovement.instance.ShowSword1(); 
        }
        else if (name.Equals("Sword 2"))
        {
            PlayerMovement.instance.ShowSword2();
        }
        else if (name.Equals("Sword 3"))
        {
            PlayerMovement.instance.ShowSword3();
        }
        else if (name.Equals("Sword 4"))
        {
            PlayerMovement.instance.ShowSword4();
        }
        else if (name.Equals("Sword 5"))
        {
            PlayerMovement.instance.ShowSword5();
        }
        else if (name.Equals("Sword 6"))
        {
            PlayerMovement.instance.ShowSword6();
        }
        else if (name.Equals("Sword 7"))
        {
            PlayerMovement.instance.ShowSword7();
        }
        else if (name.Equals("Sword 8"))
        {
            PlayerMovement.instance.ShowSword8();
        }
        else if (name.Equals("Sword 9"))
        {
            PlayerMovement.instance.ShowSword9();
        }



    }
    public void RemoveItemformInventory()
    {
        Inventory.instance.Remove(this);
    }

    internal static void SetActive(bool v)
    {
        throw new NotImplementedException();
    }
}
