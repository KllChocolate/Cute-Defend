using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    public int maxHealth;
    public int currentHealth;
    public GameObject dieUI;

    private AudioSource audioSource;
    public AudioClip HitSound;
    public AudioClip DeathSound;
    public AudioClip DefendSound;
    public AudioClip HealSound;
    public AudioClip PotionSound;
    public AudioClip gameover;

    private Animator animator;
    public HealthBar healthbar;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    public void Update()
    {
        healthbar.SetCurrentHealth(currentHealth);
    }
    public void TakeDamage(int damage)
    {

        if (PlayerMovement.instance.isDefend == true)
        {
            currentHealth = currentHealth - (damage * 0);
            animator.SetTrigger("DefendHit");
            audioSource.PlayOneShot(DefendSound);
        }
        else
        {
            currentHealth = currentHealth - damage; 

            if (currentHealth <= 0)
            {
                animator.SetTrigger("Death");
                GetComponent<Collider>().enabled = false;
                GetComponent<PlayerMovement>().enabled= false;
                audioSource.PlayOneShot(DeathSound);
                StartCoroutine(die());
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = (true);
            }
            else
            {
                animator.SetTrigger("Hit");
                audioSource.PlayOneShot(HitSound);
            }
            
        }

    }
    public IEnumerator die()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        dieUI.SetActive(true);
        audioSource.PlayOneShot(gameover);
    }

    public void healing()
    {
        currentHealth += 100;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        audioSource.PlayOneShot(PotionSound);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Heal"))
        {
            StartCoroutine(AuraHeal());
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Heal"))
        {
            StopCoroutine(AuraHeal());
        }
    }
    private IEnumerator AuraHeal()
    {
        yield return new WaitForSeconds(1);
        while (currentHealth < maxHealth)
        {
            currentHealth += 10;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            yield return new WaitForSeconds(1f);
            if (currentHealth == maxHealth) break;

        }
    }

}
