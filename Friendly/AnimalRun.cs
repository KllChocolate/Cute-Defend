using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalRun : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;
    public float distancePlayer = 4f;
    private Animator animator;
    void Start()
    {
        agent= GetComponent<NavMeshAgent>();
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < distancePlayer)
        {
            Vector3 dirToPlay = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlay;
            agent.SetDestination(newPos);
            animator.SetBool("Run", true);
        }
    }
}
