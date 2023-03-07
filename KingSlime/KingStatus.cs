using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingStatus : MonoBehaviour
{
    public static KingStatus instance;

    public int health = 1000;
    public int maxHealth;
    public int currentHealth;
    public GameObject gameOverUI;
    private Animator animator;
    public HealthBar healthbar;

    public AudioClip HitSound;
    public AudioClip DeathSound;
    public AudioClip gameover;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        maxHealth = SetMaxHealthFormHealthlevel();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    public void Update()
    {
        healthbar.SetCurrentHealth(currentHealth);

    }

    private int SetMaxHealthFormHealthlevel()
    {
        maxHealth = health;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                animator.SetTrigger("Death");
                GetComponent<Collider>().enabled = false;
                AudioSource ac = GetComponent<AudioSource>();
                ac.PlayOneShot(DeathSound);
                StartCoroutine(die());

            }
            else
            {
                animator.SetTrigger("Hit");
                AudioSource ac = GetComponent<AudioSource>();
                ac.PlayOneShot(HitSound);
            }
            healthbar.SetCurrentHealth(currentHealth);

    }
    public IEnumerator die()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        AudioSource ac = GetComponent<AudioSource>();
        ac.PlayOneShot(gameover);
    }

}
