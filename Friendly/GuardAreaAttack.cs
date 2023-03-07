using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAreaAttack : MonoBehaviour
{
    public int damage = 10;


    private void OnTriggerEnter(Collider collider)
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
