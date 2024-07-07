using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRangedWeapon : MonoBehaviour
{
    [Header("Main Weapon")]
    public bool useAlternateFireButton = false;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Transform fireMuzzleEffect;
    public float manaUsagePerShot = 10f;
    public PlayerMana manaController;

    [Header("Weapon Stats")]
    public float fireRate = 3f;

    [Header("Burst fire")]

    public bool burstFire = false;
    public int burstCount = 3;
    public float burstDelay = 0.15f;

    private float lastFired;

    // Acquires weapon sprite and also the mana controller
    private void Start() {
        transform.parent.parent.GetComponent<PlayerMovement>().weaponSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (!manaController)
        {
            manaController = transform.parent.parent.GetComponent<PlayerMana>();
        }
    }

    private void Update()
    {
        // Make sure we are not clicking UI
        var currentCurrentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (currentCurrentSelectedGameObject && currentCurrentSelectedGameObject.activeSelf) return;


        // Binding keybinds to shoot
        if (!useAlternateFireButton)
        {
            if (Input.GetButton("Fire1") && Time.time - lastFired > 1 / fireRate) Shoot();
        }
        else
        {
            if (Input.GetButton("Fire2") && Time.time - lastFired > 1 / fireRate) Shoot();
        }
    }

    // Shoot function
    private void Shoot()
    {
        lastFired = Time.time;

        if (!burstFire)
        {
            // Shoot normally
            CreateProjectile();
        }
        else
        {
            // Shoots in burst
            StartCoroutine(BurstShoot());
        }
    }

    // Shooting in bursts for some specific weapons
    private IEnumerator BurstShoot()
    {
        for (int i = 0; i < burstCount; i++)
        {
            CreateProjectile();

            yield return new WaitForSeconds(burstDelay);
        }
    }

    // Creates the projectile
    private void CreateProjectile()
    {
        manaController.SubtractMana(manaUsagePerShot);
        Instantiate(fireMuzzleEffect, firePoint.position, firePoint.rotation);
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
