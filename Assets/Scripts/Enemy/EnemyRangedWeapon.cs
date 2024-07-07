using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedWeapon : MonoBehaviour
{
    public Transform target;

    [Header("Main Weapon")]

    public Transform firePoint;
    public GameObject projectilePrefab;
    public Transform fireMuzzleEffect;

    public bool testLos = true;

    [Header("Stats")]

    public float fireRate = 1f;
    public float range = 5f;
    private float lastFired;

    // Acquire the player as the target at the start 
    void Start()
    {
        if (!target)
            target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks player distance and fires if in range
        float playerDist = Vector3.Distance(transform.position, target.position);
        if (Time.time - lastFired > 1 / fireRate && range >= playerDist)
        {
            lastFired = Time.time;
            CreateProjectile();
        }
    }

    // Create the projectile along with the muzzle effect
    private void CreateProjectile()
    {
        Instantiate(fireMuzzleEffect, firePoint.position, firePoint.rotation);
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
