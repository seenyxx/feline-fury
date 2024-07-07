using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile")]

    public Rigidbody2D rb;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 5f;
    public float damage = 10f;
    
    public Transform impactEffect;

    public bool passThroughOnInvincibilityTick = true;
    public bool useImpactEffect = true;
    public bool infinitePierce = false;
    private readonly List<int> ignoreLayers = new() { 8, 11, 12, 20, 13, 14 };

    private void Start()
    {
        // Applies a velocity onto the e nemy projectile once at the start
        rb.velocity = transform.right * projectileSpeed;
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        damage = Mathf.Round(gm.enemyDamageMultipliier * damage);
        StartCoroutine(SelfDestruct());
    }

    // Check collisions and damages the player if the collided gameobject is the player
    private void OnTriggerEnter2D(Collider2D targetInfo) {
        if (ignoreLayers.Contains(targetInfo.gameObject.layer))
        {
            return;
        }

        if (targetInfo.name == "Player")
        {
            PlayerHealth playerHp = targetInfo.GetComponent<PlayerHealth>();

            if (playerHp.isInvincible && passThroughOnInvincibilityTick)
            {
                return;
            }

            // Damages player if the player is not invincible
            playerHp.SubtractHealth(damage);
        }

        if (!infinitePierce)
            DestroyProjectile();
    }

    // Automatic removal of the projectile after a certain amount of time
    private IEnumerator SelfDestruct()
    {
       // Wait for x amount of seconds that the projectile will destroy itself after
        yield return new WaitForSeconds(projectileLifetime);
        // Destroys the object after time is up
        DestroyProjectile();
    }

    // Destroy the projectile while having impact effects
    public void DestroyProjectile()
    {
        if (useImpactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
