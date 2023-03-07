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
    public LayerMask whatIsTarget, whatIsPlayer;

    //Attacking
    public float cooldown;
    public bool readyAttack = true;
    public bool isAttacking = false;
    public bool isDead = false;
    public bool isAllEnemiesDead = false;

    //States
    public float sightPlayerRange, sightTargetRange, attackRange;
    public float sightGuardRange = 99;
    public bool targetInSightRange, targetInAttackRange, playerInSightRange, playerInAttackRange;
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
        target = GameObject.Find("Slime King").transform;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }
    private void Update()
    {
        healthbar.SetCurrentHealth(currentHealth);

        targetInSightRange = Physics.CheckSphere(transform.position, sightTargetRange, whatIsTarget);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsTarget);
        playerInSightRange = Physics.CheckSphere(transform.position, sightPlayerRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!isDead)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Guard");
            List<GameObject> enemiesInRange = new List<GameObject>();

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < sightGuardRange)
                {
                    enemiesInRange.Add(enemy);
                }
            }
            if (enemiesInRange.Count > 0 && !playerInSightRange)//ถ้ามากกว่า 0 ตัว
            {
                GameObject closestEnemy = enemiesInRange[0];
                float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                foreach (GameObject enemy in enemiesInRange)
                {
                    if (enemy.CompareTag("Guard") && enemy.activeSelf && !enemy.CompareTag("Dead"))
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
                        attack(gameObject);
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
            if (isAllEnemiesDead && targetInSightRange)
            {
                if (targetInSightRange && !playerInSightRange && !targetInAttackRange)
                {
                    Chase(target);
                }
                if (targetInAttackRange && readyAttack)
                {
                    animator.SetBool("Run", false);
                    Attack(target);
                }
            }
            if (playerInSightRange)
            {   
                if (playerInSightRange && !playerInAttackRange)
                {
                    Chase(player);
                }
                if (playerInAttackRange && readyAttack)
                {
                    animator.SetBool("Run", false);
                    Attack(player);
                }
                
            }
            if (IsAllEnemiesDead())
            {
                isAllEnemiesDead = true;
            }
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
    private void attack(GameObject enemy)
    {
        animator.SetBool("Run", false);
        audioSource.PlayOneShot(attackSound);
        agent.SetDestination(enemy.transform.position);
        transform.LookAt(enemy.transform.position);
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0, 2));
        isAttacking = true;
        readyAttack = false;
        StartCoroutine(AttackCooldown());
    }

    private void Attack(Transform destination)
    {
        animator.SetBool("Run", false);
        audioSource.PlayOneShot(attackSound);
        agent.SetDestination(destination.transform.position);
        transform.LookAt(destination);
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0, 2));
        isAttacking = true;
        readyAttack = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        EnemyAreaAttack.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
        EnemyAreaAttack.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        readyAttack = true;

    }
    private bool IsAllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Walkaround>().currentHealth > 0)
            {
                return false;
            }
        }
        return true;
    }
}
