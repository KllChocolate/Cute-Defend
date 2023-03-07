using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaAttack : MonoBehaviour
{
    public int damage;

    void Update()
    {
        damage = AIMutiTarget.instance.damage;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "King")
        {
            KingStatus king = collider.GetComponent<KingStatus>();
            if (king != null) king.TakeDamage(damage);
        }
        if (collider.tag == "Player")
        {
            PlayerStatus playerStatus = collider.GetComponent<PlayerStatus>();
            if (playerStatus != null) playerStatus.TakeDamage(damage);
        }
        if (collider.tag == "Guard")
        {
            Walkaround walkaround = collider.GetComponent<Walkaround>();
            if (walkaround != null) walkaround.TakeDamage(damage);

        }
    }
}
