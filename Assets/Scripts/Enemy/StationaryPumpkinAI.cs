using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class StationaryPumpkinAI : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 3f;
    public bool useRngAtkCooldown = true;
    public float maxAtkCooldown = 5f;
    public float minAtkCooldown = 3f;
    private float atkTimer = 0f;
    
    [Header("Projectile & Effects")]
    public GameObject pumpkinProjectile;
    public ParticleSystem attackParticles;


    // Randomise the sprite flipping and the cooldown of the attack at the start
    private void Start() {
        RandomiseCooldown();
        int flip = Random.Range(0, 2);
        GetComponent<SpriteRenderer>().flipX = flip != 0;
    }

    // Randomises the attack cooldown
    private void RandomiseCooldown ()
    {
        attackCooldown = useRngAtkCooldown ? Random.Range(maxAtkCooldown, minAtkCooldown) : attackCooldown;
    }

    // Check if the cooldown has expired every single tick of the game
    private void FixedUpdate() {
        atkTimer += Time.fixedDeltaTime;

        if (atkTimer > attackCooldown)
        {

            Attack();
            RandomiseCooldown();
            atkTimer = 0f;
        }
    }

    // Attack animations and particles
    private void Attack()
    {
        attackParticles.Play();
        Instantiate(pumpkinProjectile, transform.position, Quaternion.identity);
    }
}
