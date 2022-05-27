using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Rigidbody body;  
    public float ghostMoveSpeed;
    void Start()
    {

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
}
