using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Transform player;
   // public float ghostMoveSpeed;
    public float health = 50f;
    private float currenthealth;
    Break_Ghost breakGhost;
    public GameObject projectile;
    public NavMeshAgent agent;
    public LayerMask whatIsPlayer = new LayerMask();
    private float timeBetweenAttacks = 1f;
    private bool alreadyAttacked;
    private float projectileForce = 20f;


    private float attackRange;
    private bool playerInAttackRange;


    private void Start()
    {
        currenthealth = health;
        attackRange = agent.stoppingDistance;
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        MoveEnemyTowardsPlayer();
    }

    private void MoveEnemyTowardsPlayer()
    {
        agent.SetDestination(player.position);
        agent.transform.position = new Vector3(agent.transform.position.x, 
                                               player.position.y-0.3f, 
                                               agent.transform.position.z);
        transform.LookAt(player);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(playerInAttackRange)
        {
            AttackPlayer();
        }
        
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        currenthealth -= damage;
        if (currenthealth <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        KilledEnemies.currentEnemies--;
        this.gameObject.tag = "NotEnemy";
        breakGhost = this.gameObject.GetComponent<Break_Ghost>();
        breakGhost.break_Ghost();
        currenthealth = health;
        this.gameObject.GetComponent<Enemy>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        Destroy(this.gameObject,3f);
    }
}
