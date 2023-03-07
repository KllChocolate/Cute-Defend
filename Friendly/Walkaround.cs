using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Walkaround : MonoBehaviour
{
    public float speed;
    public int maxHealth;
    public int currentHealth;

    public AudioSource audioSource;
    public AudioClip HitSound;
    public AudioClip DeathSound;
    public AudioClip attackSound;

    public GameObject hpbar;
    public GameObject guardAreaAttack;
    public HealthBar healthbar;
    private Animator animator;
    private NavMeshAgent agent;
    
    public LayerMask whatIsTarget, whatIsGround;

    //Attacking
    public float cooldown;
    public bool readyAttack = true;
    public bool isAttacking = false;
    public float lastAttackTime = -Mathf.Infinity;
    public bool isdead = false; 

    //States
    public float sigthTargetRang, attackRange;

      
    public void Start()
    { 
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponentInChildren<AudioSource>();
        speed = agent.speed;
    }
    public void Update()
    {
        if (!isdead)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            List<GameObject> enemiesInRange = new List<GameObject>();

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < sigthTargetRang)
                {
                    enemiesInRange.Add(enemy);
                }
            }
            if (enemiesInRange.Count > 0)//ถ้ามากกว่า 0 ตัว
            {
                GameObject closestEnemy = enemiesInRange[0];
                float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                foreach (GameObject enemy in enemiesInRange)
                {
                    if (enemy.CompareTag("Enemy") && enemy.activeSelf && !enemy.CompareTag("Dead"))
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distance < closestDistance)
                        {
                            closestEnemy = enemy;
                            closestDistance = distance;
                        }
                    }
                }
                if (closestDistance > attackRange)
                {
                    agent.SetDestination(closestEnemy.transform.position);
                    animator.SetBool("Run", true);
                }
                else if (closestDistance < attackRange)
                {
                    if (readyAttack)
                    {
                        agent.SetDestination(closestEnemy.transform.position);
                        animator.SetBool("Run", false);
                        Attack(closestEnemy);
                        transform.LookAt(closestEnemy.transform);
                    }
                }
                else
                {
                    agent.SetDestination(closestEnemy.transform.position);
                    animator.SetBool("Run", false);
                    transform.LookAt(closestEnemy.transform);
                }

            }
            else animator.SetBool("Run", false);
        }
    }

    private void Attack(GameObject enemy)
    {
        animator.SetBool("Run", false);
        audioSource.PlayOneShot(attackSound);
        transform.LookAt(enemy.transform.position);
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0, 2));
        isAttacking = true;
        readyAttack = false;
        StartCoroutine(ResetAttack());
        
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        guardAreaAttack.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
        guardAreaAttack.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        readyAttack = true;
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sigthTargetRang);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        healthbar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetBool("Death" ,true);
            GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(DeathSound);
            StartCoroutine(DeathTime());
            hpbar.SetActive(false);
            readyAttack = false;
            isdead = true;
            GetComponent<Walkaround>().enabled = false;
        }
        else
        {
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(HitSound);
        } 
    }
    IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }


}
