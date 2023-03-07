using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMutiTarget : MonoBehaviour
{
    public int damage;
    public int maxHealth;
    public int currentHealth; 
    public GameObject[] items;
    public GameObject EnemyAreaAttack;

    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    public HealthBar healthbar;
    public GameObject hpbar;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform target;
    private Transform player;
    private Transform guard;
    public LayerMask whatIsTarget, whatIsPlayer, whatIsGuard;

    //Attacking
    public float cooldown;
    public bool readyAttack = true;
    public bool isAttacking = false;
    public bool isDead = false;

    //States
    public float sightPlayerRange, sightTargetRange, sightGuardRange, attackRange;
    public bool targetInSightRange, targetInAttackRange, playerInSightRange, playerInAttackRange,guardInSightRange, guardInAttackRange;
    public static AIMutiTarget instance;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        EnemyAreaAttack.SetActive(false);
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        guard = GameObject.Find("Guard").transform;
        target = GameObject.Find("Slime King").transform;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }
    private void Update()
    {
        if (!isDead)
        {
            healthbar.SetCurrentHealth(currentHealth);

            targetInSightRange = Physics.CheckSphere(transform.position, sightTargetRange, whatIsTarget);
            targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsTarget);
            guardInSightRange = Physics.CheckSphere(transform.position, sightGuardRange, whatIsGuard);
            guardInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsGuard);
            playerInSightRange = Physics.CheckSphere(transform.position, sightPlayerRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (targetInSightRange && !guardInSightRange && !playerInSightRange && !targetInAttackRange)
            {
                Chase(target);
            }
            if (targetInAttackRange && readyAttack)
            {
                Attack();
            }
            if (playerInSightRange && !guardInSightRange &&!playerInAttackRange)
            {
                Chase(player);
            }
            if ( playerInAttackRange && readyAttack)
            {
                Attack();
            }
            if (guardInSightRange && !guard.CompareTag("Dead") && !guardInAttackRange)
            {
                Chase(guard);
            }
            if (guardInAttackRange && readyAttack)
            {
                Attack();
            }
        }
        else
        {
            targetInSightRange = false;
            targetInAttackRange = false;
            playerInSightRange = false;
            playerInAttackRange = false;
            guardInSightRange = false;
            guardInAttackRange = false;
        }
    }
    private void Chase(Transform destination)
    {
        agent.SetDestination(destination.position);
        animator.SetBool("Run", true);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightPlayerRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightGuardRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightTargetRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        healthbar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            gameObject.tag = "Dead";
            animator.SetBool("Run", false);
            readyAttack = false;
            GetComponent<Collider>().enabled = false;
            animator.SetBool("Death" , true);
            audioSource.PlayOneShot(deathSound);
            Invoke("SpawnItem", 0.1f);
            StartCoroutine(DestroyTime());
            hpbar.SetActive(false);
            GetComponent<AIMutiTarget>().enabled = true;

        }
        else
        {
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);
        }

    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }

    void SpawnItem()
    {
        foreach (var item in items)
        {
            item.SetActive(true);
        }

    }

    private void Attack()
    {
        animator.SetBool("Run", false);
        audioSource.PlayOneShot(attackSound);
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0, 2));
        isAttacking = true;
        readyAttack = false;
        StartCoroutine(AttackCooldown());
        EnemyAreaAttack.SetActive(true);
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
        EnemyAreaAttack.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        readyAttack = true;

    }
}
