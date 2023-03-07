using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraHeal : InteractiveObject
{

    public override void Aura()
    {
        base.Aura();
        InvokeRepeating("Heal", 0f, 1f);
    } 
    public override void StopAura()
    {
        base.StopAura();
        CancelInvoke("Heal");
    }
    
    void Heal()
    {
        PlayerStatus.instance.AuraHeal();
    }
}
