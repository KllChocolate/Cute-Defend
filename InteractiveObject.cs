using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public float radius = 3f;
    public Transform player;
    public Transform interactItem;
    bool hasInteract = false;
    public void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        float distance = Vector3.Distance(player.position, interactItem.position);
        if (distance <= radius && !hasInteract)
        {
            hasInteract = true;
            Interact();
            Aura();

        }
        if (distance > radius && hasInteract) 
        { 
            hasInteract= false;
            StopAura();
        }
    }
    public virtual void Interact(){}
    public virtual void Aura(){}
    public virtual void StopAura(){}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactItem.position,radius);
    }
}
