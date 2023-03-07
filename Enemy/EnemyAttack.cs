using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage);
            }
        }
    }
}
