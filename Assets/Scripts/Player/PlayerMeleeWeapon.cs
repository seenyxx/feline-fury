using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMeleeWeapon : MonoBehaviour
{
    private Transform weaponHolder;
    private SpriteRenderer weaponSprite;
    private WeaponAim weaponAim;

    public int enemyLayer = 12;
    [Header("Weapon Properties")]
    public bool primaryAttack = true;
    public float weaponSpeed = 3f;
    public float weaponDamage = 30f;
    public float weaponCritMulti = 1.5f;
    public Transform contactParticles;
    public Collider2D swordCollider;
    public List<int> attackedObjects;

    [Range(0f, 1f)]
    public float critChance = 0.25f;
    private float lastUse;

    [Header("Mana Usage")]
    public bool useMana = false;
    public float manaUsage = 5f;
    private PlayerMana manaController;

    [Header("Lifesteal")]
    public bool lifesteal = false;
    [Range(0f, 1f)]
    public float lifeStealChance = 0.75f;
    public float lifeStealAmount = 3f;
    public PlayerHealth playerHealth;
    public Transform lifeStealDisplay;


    [Header("Parry")]
    public bool parryProjectiles = false;
    public int enemyProjectilesLayer = 8;
    public Transform parryText;

    [Header("Secondary Attack")]
    public bool secondaryAttack = false;
    public bool swapAttackTriggers = false;
    public float retractDistance = 0.5f;
    public float spearingSpeed = 5f;
    public float retractingSpeed = 2f; 
    public float thrustDistance = 2f;

    public bool useDifferentDamage = true;
    public float secondaryDamage = 20f;
    public float secondaryCritMulti = 2f;
    
    [Range(0f, 1f)]
    public float secondaryCritChance = 0.7f;
    private bool currentlySecondaryAttacking = false;

    public bool instantiateSecondaryAtkParticle = true;
    public Transform secondaryAttackParticle;
    public Transform firePoint;


    [Header("Swing Animation")]
    public bool rotateClockwise = true;
    public float targetRotation = 90f;
    public float rotationSpeed = 90f;
    public float initialAccel = 5f;
    public float maxAcceleration = 50f;
    public TrailRenderer swordTrail;

    public List<TrailRenderer> swordTrails;
    public float defaultRotation = -30f;

    private float currentSpeed = 0f;
    private float currentRotation = 0f;
    private float currentAcceleration = 0f;
    private bool lockFlip = false;
    private bool currentlyAttacking = false;
    
    [Header("Damage Physics")]
    public bool damagePhysics = true;
    public float pushDuration = 0.1f;
    public float pushForce = 10f;

    private AudioSource audioSource;


    // Acquires all the components of the weapon and everything that is requried for the weapon to function
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weaponHolder = transform.parent;
        weaponAim = weaponHolder.GetComponent<WeaponAim>();
        weaponSprite = GetComponent<SpriteRenderer>();
        playerHealth = transform.parent.parent.GetComponent<PlayerHealth>();
        if (!swordCollider) swordCollider = GetComponent<Collider2D>();

        if (!manaController && useMana)
        {
            manaController = transform.parent.parent.GetComponent<PlayerMana>();
        }

        ResetRotation();
    }

    // Resets the attack and allows for new attack
    private void ResetAttack()
    {
        attackedObjects.Clear();
        currentlyAttacking = false;
        lockFlip = false;
        currentRotation = 0f;
        currentSpeed = 0f;
        currentAcceleration = 0f;
        ResetRotation();
    }

    // Resets the rotation of the weapon back to original
    private void ResetRotation()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, weaponHolder.localRotation.z < 0 ? defaultRotation : -defaultRotation);
    }

    // Sets the state of the sword emission and allows for multiple emissive elements
    private void SetSwordTrailEmission(bool value)
    {
        if (swordTrails.Count > 0)
        {
            foreach (var st in swordTrails)
            {
                st.emitting = value;
            }
        }
        else
        {
            swordTrail.emitting = value;
        }
    }

    // Animates the weapon swing
    private IEnumerator AnimateSwing()
    {
        SetSwordTrailEmission(true);
        lockFlip = true;
        int rotationDirection = rotateClockwise ? 1 : -1;
        // float timeToReachTarget = Mathf.Sqrt(2 * targetRotation / rotationAccel);

        while (Mathf.Abs(currentRotation) < Math.Abs(targetRotation))
        {
            if (currentAcceleration < maxAcceleration)
            {
                currentAcceleration += initialAccel * Time.deltaTime;
                currentAcceleration = Mathf.Min(currentAcceleration, maxAcceleration);
            }

            currentSpeed += currentAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, rotationSpeed);

            float rotationThisFrame = currentSpeed * Time.deltaTime * rotationDirection;

            currentRotation += rotationThisFrame;

            transform.Rotate(Vector3.forward * rotationThisFrame);

            yield return null;
        }
        
        SetSwordTrailEmission(false);

        yield return new WaitForSeconds(0.05f);
        ResetAttack();
    }

    // Swing attack while subtracting mana and updating the flags
    private void Attack()
    {
        if (!primaryAttack) return;
        currentlyAttacking = true;

        lastUse = Time.time;

        if (useMana)
        {
            manaController.SubtractMana(manaUsage);
        }

        audioSource.Play();
        CheckForEnemiesInTrigger();
        StartCoroutine(AnimateSwing());
    }

    // Animates the stab attack
    private IEnumerator AnimateStab()
    {
        lockFlip = true;
        weaponAim.rotate = false;
        weaponAim.InstantRotate();
        transform.localRotation = Quaternion.identity;
        
        Vector2 originalPosition = Vector2.zero;

        currentlySecondaryAttacking = true;
        currentlyAttacking = true;

        while (transform.localPosition.y > originalPosition.y - retractDistance)
        {
            transform.Translate(retractingSpeed * Time.deltaTime * Vector2.down);
            yield return null;
        }

        float distanceMoved = 0f;

        if (instantiateSecondaryAtkParticle)
            Instantiate(secondaryAttackParticle, firePoint.position, firePoint.rotation, transform);

        while (distanceMoved < thrustDistance)
        {
            transform.Translate(spearingSpeed * Time.deltaTime * Vector2.up);
            distanceMoved += spearingSpeed * Time.deltaTime;
            yield return null;
        }

        while (transform.localPosition.y < originalPosition.y)
        {
            transform.Translate(retractingSpeed * Time.deltaTime * Vector2.up);
            yield return null;
        }

        transform.localPosition = originalPosition;
        currentlySecondaryAttacking = false;
        ResetAttack();
        weaponAim.rotate = true;
    }

    // Stab attack while subtracting mana and also setting flags
    private void SecondaryAttack()
    {
        if (!secondaryAttack) return;

        if (useMana)
        {
            manaController.SubtractMana(manaUsage);
        }

        lastUse = Time.time;
        audioSource.Play();
        CheckForEnemiesInTrigger();
        StartCoroutine(AnimateStab());
    }

    // Constantly checks for player input and attacks based on input
    void Update()
    {
        if (currentlyAttacking) return;
        var currentCurrentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (currentCurrentSelectedGameObject && currentCurrentSelectedGameObject.activeSelf) return;

        if (Input.GetButton("Fire1") && Time.time - lastUse > 1 / weaponSpeed) 
        {
            if (!swapAttackTriggers)
                Attack();
            else
                SecondaryAttack();
        }

        if (Input.GetButton("Fire2") && Time.time - lastUse > 1 / weaponSpeed) {
            if (!swapAttackTriggers)
                SecondaryAttack();
            else
                Attack();
        }
    }

    // Constantly performs the rotation every game tick if there is rotation being applied
    private void FixedUpdate() {
        if (!lockFlip)
        {
            weaponSprite.flipX = weaponHolder.localRotation.z < 0;
            ResetRotation();
        }
        
        rotateClockwise = weaponHolder.localRotation.z < 0;
    }

    // Pushes the enemy being attacked and contacted
    private IEnumerator PushObject(GameObject obj)
    {
        Vector2 direction = (obj.transform.position - transform.position).normalized;
        float elapsedTime = 0f;

        while (elapsedTime < pushDuration && obj)
        {
            obj.transform.position += (Vector3)(pushForce * Time.deltaTime * direction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Damages the enemy and evaluates the chance for a critical hit
    private void DamageEnemy(GameObject other)
    {
        if (other && damagePhysics)
            StartCoroutine(PushObject(other));

        var enemyHp = other.GetComponent<EnemyHealth>();
        if (currentlySecondaryAttacking && useDifferentDamage)
        {
            if (UnityEngine.Random.value < secondaryCritChance)
            {
                enemyHp.SubtractHealth(secondaryDamage * secondaryCritMulti, true);
            }
            else
            {
                enemyHp.SubtractHealth(secondaryDamage);
            }
        }
        else
        {
            if (UnityEngine.Random.value < critChance)
            {
                enemyHp.SubtractHealth(weaponDamage * weaponCritMulti, true);
            }
            else
            {
                enemyHp.SubtractHealth(weaponDamage);
            }
        }
    }

    // Check if the enemy is able to be hit
    private void CheckEnemy(GameObject other)
    {
        if (currentlyAttacking && other.layer == enemyLayer)
        {
            int id = other.GetInstanceID();

            if (attackedObjects.Contains(id)) return;

            attackedObjects.Add(id);
            
            Instantiate(contactParticles, other.transform.position, Quaternion.identity);
            DamageEnemy(other);

            if (lifesteal && UnityEngine.Random.value < lifeStealChance)
            {
                playerHealth.AddHealth(lifeStealAmount);
                
                var healDisplay = Instantiate(lifeStealDisplay, transform.position, Quaternion.identity);
                healDisplay.GetComponent<TextMeshPro>().text = $"+ {lifeStealAmount}";
            }
        }
    }

    // Check if projectiles are able to be parried
    private void CheckParry(GameObject other)
    {
        if (parryProjectiles && other.layer == enemyProjectilesLayer && currentlyAttacking)
        {
            EnemyProjectile proj = other.GetComponent<EnemyProjectile>();
            proj.damage = 0f;
            proj.DestroyProjectile();

            Instantiate(contactParticles, other.transform.position, Quaternion.identity);
            Instantiate(parryText, other.transform.position, Quaternion.identity);
        }
    }

    // Check for any enemies within the collider already prior to attack
    public void CheckForEnemiesInTrigger()
    {
        List<Collider2D> colliders = new();

        int colliderCount = swordCollider.Overlap(colliders);

        if (colliderCount < 1) return;

        foreach (Collider2D other in colliders)
        {
            CheckParry(other.gameObject);
            CheckEnemy(other.gameObject);
        }
    }


    // Check for enemies and projectiles when they enter the trigger
    private void OnTriggerEnter2D(Collider2D other) {
        CheckParry(other.gameObject);
        CheckEnemy(other.gameObject);
    }

    // Check for enemies and projectiles when they stay in the trigger
    private void OnTriggerStay2D(Collider2D other) {
        CheckParry(other.gameObject);
        CheckEnemy(other.gameObject);
    }
    
    // Check for enemies and projectiles when they exit the trigger
    private void OnTriggerExit2D(Collider2D other) {
        CheckParry(other.gameObject);
        CheckEnemy(other.gameObject);
    }
}

