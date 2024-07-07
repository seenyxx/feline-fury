using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RatBoss : MonoBehaviour
{
    public Animator animator;
    public float maxAttackDelay = 8f;
    public float minAttackDelay = 3f;
    public Sprite attackSprite;
    public EnemyHealth enemyHealth;
    public SpriteRenderer spriteRenderer;
    float attackInterval = 5f;
    public float lockRadius = 5f; 
    public float invincibilityTime = 3f;
    public Transform centrePoint;

    [Header("Enemies")]
    public List<Transform> spawners;
    public Transform spawnPtParent;
    public GameObject spawnFx;

    public Transform[] enemyPrefabs;
    public int maxEnemiesPerWave = 5;
    public int minEnemiesPerWave = 2;

    // Acquire the spawn points of enemies
    private void Start() {
        foreach (Transform child in spawnPtParent.GetComponentInChildren<Transform>())
        {
            spawners.Add(child);
        }

        RandomiseAttack();
    }

    // Clamp the position of the boss within a radius
    private void Update() {
        Vector2 offset = transform.position - centrePoint.position;
        
        // Check if the object is outside the radius
        if (offset.magnitude > lockRadius)
        {
            offset = offset.normalized * lockRadius;
            transform.position = (Vector2)centrePoint.position + offset;
        }
    }

    // Creates a loop where the boss will constantly attack
    private void RandomiseAttack()
    {
        attackInterval = Random.Range(minAttackDelay, maxAttackDelay);
        StartCoroutine(StartAttack());
    }

    // Starts the actual attack after a time delay and also makes the boss invulnerable for a certain amount of time
    private IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(attackInterval);
        Attack();

        yield return new WaitForSeconds(invincibilityTime);
        animator.speed = 1;
        animator.enabled = true;
        enemyHealth.enabled = true;
        RandomiseAttack();
    }

    // Performs the actual attack
    private void Attack()
    {
        // Freezes the boss and sprite
        animator.speed = 0;
        animator.enabled = false;
        enemyHealth.enabled = false;
        spriteRenderer.sprite = attackSprite;

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        Transform randomEnemy = enemyPrefabs[randomEnemyIndex];
        int randomEnemyCount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave);

        // Spawns the enemies
        for (int i = 0; i < randomEnemyCount; i++)
        {
            int randomSpawnPtIndex = Random.Range(0, spawners.Count);
            Transform spawner = spawners[randomSpawnPtIndex];


            Instantiate(spawnFx, spawner.position, Quaternion.identity);
            Instantiate(randomEnemy, spawner.position, Quaternion.identity);
        }
    }
}
