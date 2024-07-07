using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySpawner : MonoBehaviour
{
    public Transform player;
    public float spawnDistance = 3f;
    public Transform spawnEffect;
    public Transform spawnObject;

    public LayerMask rayCastLayers;
    
    public bool useEnemyManager = true;
    public string enemyManagerId;
    public bool useRaycast = false;

    private EnemyManager enemyManager;

    // Acquires teh enemy maanger and also player to target
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        if (!player)
        {
            player = GameObject.Find("Player").transform;
        }
    }

    // Constantly check line of sight and distance every tick
    private void FixedUpdate()
    {
        if (!useRaycast) 
        {
            CheckDistance();
        }
        else
        {
            CheckRayCast();
        }
    }
    

    // Checks if player is in line of sight
    private void CheckRayCast()
    {
        Vector3 playerDirection = (player.position - transform.position).normalized;

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, playerDirection, 10f, rayCastLayers);

        if (raycast.transform == player)
        {
            SummonEnemy();
        }
    }

    // Checks for player distance
    private void CheckDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= spawnDistance)
        {
            SummonEnemy();
        }
    }

    // Summons the actual enemy 
    private void SummonEnemy()
    {
        if (!enemyManager.CheckSpawn(enemyManagerId) && useEnemyManager)
        {
            return;
        }

        if (useEnemyManager)
            enemyManager.IncrementEnemyCount(enemyManagerId);
        
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Instantiate(spawnObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
