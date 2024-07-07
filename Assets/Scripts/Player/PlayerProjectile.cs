using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile")]
    public bool damaging = true;
    public bool infinitePierce = false;
    public Rigidbody2D rb;
    public float projectileSpeed = 20f;
    public float projectileLifetime = 5f;
    public float damage = 10f;
    public int enemyLayer = 12;
    
    public Transform impactEffect;

    public int[] ignoreLayers = new int[] { 3, 13, 14};
    public int overlayLayer = 20;

    [Header("Critical Damage")]
    public float critDmgMultiplier = 2f;

    [Range(0f, 1f)]
    public float critChance = 0.25f;

    // Applies a velocity on the projectile on start
    private void Start()
    {
        rb.velocity = transform.right * projectileSpeed;
        
        // Starts the automatic self destruct
        StartCoroutine(SelfDestruct());
    }

    // Check for collisions
    private void OnTriggerEnter2D(Collider2D targetInfo) {
        // Damages enemies
        if (ignoreLayers.Contains(targetInfo.gameObject.layer) || targetInfo.gameObject.layer == overlayLayer)
        {
            return;
        }

        if (targetInfo.gameObject.layer == enemyLayer && damaging)
        {
            var enemyHp = targetInfo.GetComponent<EnemyHealth>();

            if (Random.value < critChance)
            {
                enemyHp.SubtractHealth(damage * critDmgMultiplier, true);
            }
            else
            {
                enemyHp.SubtractHealth(damage);
            }
        }

        // Check if the projectile can infinitely pierce
        if (!infinitePierce)
            DestroyProjectile();
    }

    // Self destruct after a certain amount of time
    private IEnumerator SelfDestruct()
    {
       // Wait for x amount of seconds that the projectile will destroy itself after
        yield return new WaitForSeconds(projectileLifetime);
        // Destroys the object after time is up
        DestroyProjectile();
    }

    // Destroys projectile while creating an impact effect
    private void DestroyProjectile()
    {
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
