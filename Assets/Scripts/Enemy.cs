using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public Rigidbody body;  
    public float ghostMoveSpeed;
    public float health = 50f;
    private float currenthealth;
    Break_Ghost breakGhost;
    public BoxCollider collider1st;
    public BoxCollider collider2nd;

    private void Start()
    {
        currenthealth = health;
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        MoveEnemyTowardsPlayer();
    }

    private void MoveEnemyTowardsPlayer()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, player.position, ghostMoveSpeed * Time.deltaTime);
        body.MovePosition(pos);
        transform.LookAt(player);
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
        collider1st.enabled = false;
        collider2nd.enabled = false;
        currenthealth = health;
        Destroy(this.gameObject,3f);
    }
}
