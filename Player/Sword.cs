using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage;
    
    public static Sword instance;

    public void Awake()
    {
        instance = this; 
    }
}
