using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{

    private GameObject player;
    NavMeshAgent agent;
    Animator animator;
    GameManager manager;

    private bool hasCollided = false;
    private float minimumDistance = 5f;
    float countDownTime = 1f;
    bool isCounting = false;

    public bool returnCollided()
    {
        return hasCollided;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();   
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!manager.gameOver)
        {
            if (!hasCollided)
            {
                actionAgent();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance >= minimumDistance)
                {
                    hasCollided = false;
                    actionAgent();
                }
            }
        }
        else
        {
            hasCollided = false;
            agent.destination = transform.position;
            animator.SetFloat("Speed_f", 0);
            StopAllCoroutines();
        }
        
    }

    void actionAgent()
    {
        agent.destination = player.transform.position;
        animator.SetFloat("Speed_f", agent.velocity.magnitude);
        //animator.SetBool("Bark_b", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasCollided = true;
            agent.destination = transform.position;
            // animator.SetBool("Bark_b", true);
            animator.SetFloat("Speed_f", 0);
            if (!isCounting)
            {
                isCounting = true;
                StartCoroutine(CountdownRoutine());
            }
        }     
    }

    IEnumerator CountdownRoutine()
    {
        while (hasCollided)
        {
            int currentHealth = player.GetComponent<PlayerManager>().returnCurrentHealth();
            currentHealth -= 1;
            player.GetComponent<PlayerManager>().setHurtAudioSource();
            player.GetComponent<PlayerManager>().setCurrentHealth(currentHealth);
            player.GetComponent<PlayerManager>().healthBar.setHealth(currentHealth);
            yield return new WaitForSeconds(countDownTime);
        }
        isCounting = false;
    }
}
