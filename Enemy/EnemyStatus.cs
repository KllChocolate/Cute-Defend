using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR;

public class EnemyStatus : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public GameObject[] items;

    public AudioClip HitSound;
    [Range(0, 1)] public float HitSoundAudioVolume = 0.5f;
    public AudioClip DeathSound;
    [Range(0, 1)] public float DeathSoundAudioVolume = 0.5f;

    public Animator animator;

    public HealthBar healthbar;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        maxHealth = SetMaxHealthFormHealthlevel();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage);
            }
        }
    }

    private int SetMaxHealthFormHealthlevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthbar.SetCurrentHealth(currentHealth);


        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            GetComponent<Collider>().enabled = false;
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(DeathSound);
            Invoke("SpawnItem", 3f);

        }
        else
        {
            animator.SetTrigger("Hit");
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(HitSound);
        }
    }

    void SpawnItem()
    {
        foreach(var item in items)
        {
            item.SetActive(true);
        }
        
    }

}
