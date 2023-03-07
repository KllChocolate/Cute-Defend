using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingStatus : MonoBehaviour
{
    public static KingStatus instance;

    public int maxHealth = 1000;
    public int currentHealth;
    public GameObject gameOverUI;
    private Animator animator;
    public HealthBar healthbar;

    private AudioSource audioSource;
    public AudioClip HitSound;
    public AudioClip DeathSound;
    public AudioClip gameover;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    public void Update()
    {
        healthbar.SetCurrentHealth(currentHealth);

    }

    public void TakeDamage(int damage)
    {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                animator.SetTrigger("Death");
                GetComponent<Collider>().enabled = false;
                audioSource.PlayOneShot(DeathSound);
                StartCoroutine(die());

            }
            else
            {
                animator.SetTrigger("Hit");
                audioSource.PlayOneShot(HitSound);
            }
            healthbar.SetCurrentHealth(currentHealth);

    }
    public IEnumerator die()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        audioSource.PlayOneShot(gameover);
    }

}
