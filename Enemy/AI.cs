using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public bool IsAttacking = false;
    public GameObject[] items;

    public AudioClip HitSound;
    [Range(0, 1)] public float HitSoundAudioVolume = 0.5f;
    public AudioClip DeathSound;
    [Range(0, 1)] public float DeathSoundAudioVolume = 0.5f;

    public Animator animator;
    public HealthBar healthbar;
    public GameObject hpbar;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").transform;
        agent= GetComponent<NavMeshAgent>();
    }
    public void Start()
    {    
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
    }
    private void SearchWalkPoint()
    {
        float Z = Random.Range(-walkPointRange, walkPointRange);
        float X = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0, 2));
        IsAttacking = true;

        if (!alreadyAttacked) 
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        IsAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && IsAttacking == true)
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage);
            }
        }
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
            Invoke("SpawnItem", 1f);
            GetComponent<AI>().enabled = false;
            StartCoroutine(DeathTime());
            StartCoroutine(DestroyTime());
            hpbar.SetActive(false);

        }
        else
        {
            animator.SetTrigger("Hit");
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(HitSound);
        }
    }
    IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().enabled = false;
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(10f); 
        Destroy(gameObject); 
    }

    void SpawnItem()
    {
        foreach (var item in items)
        {
            item.SetActive(true);
        }

    }
}
