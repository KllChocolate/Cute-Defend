using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public int damage;
    public void Update()
    {
        damage = Sword.instance.damage;
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            AIMutiTarget aIMutiTarget = collider.GetComponent<AIMutiTarget>();

            if (aIMutiTarget != null)
            {
                aIMutiTarget.TakeDamage(damage);

            }
        }
    }
}
