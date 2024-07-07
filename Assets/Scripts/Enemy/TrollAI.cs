using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrollAI : MonoBehaviour
{
    [Header("Attacks")]
    public Transform target;
    public float meleeAtkDistance = 1f;
    public float atkCooldown = 1.5f;
    public Transform attackFirepoint;
    public GameObject attackPrefab;

    private bool canAtk = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AIPath aIPath;
    public AIDestinationSetter aIDestinationSetter;
    private bool flipSprite;

    // Acquire the player for targetting purposes
    void Start()
    {
        if (!target)
            target = GameObject.Find("Player").transform;
    }

    // Calculate the attack and also flip the sprite based on the direction of movement every single tick
    private void FixedUpdate()
    {
        if (canAtk)
        {
            CalculateAtk();
            animator.SetBool("Moving", aIPath.desiredVelocity != Vector3.zero);
        }

        if (aIPath.desiredVelocity.x < 0f)
        {
            flipSprite = true;
        }
        else if (aIPath.desiredVelocity.x > 0f)
        {
            flipSprite = false;
        }
    }
    
    // Enabling or disabling the movement
    private void ToggleMovement()
    {
        aIDestinationSetter.enabled = !aIDestinationSetter.enabled;
    }

    // Calculating whether or not the enemy can attack the player
    private void CalculateAtk()
    {
        if (!canAtk) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > meleeAtkDistance) return;

        canAtk = false;

        ToggleMovement();
        animator.SetTrigger("Attack");
        StartCoroutine(MeleeAttack());
        StartCoroutine(ResetAttackStatus());
    }

    // Resetting the attack and also applying delay between the attac
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(atkCooldown);
        canAtk = true;
    }

    // Performs the actual melee attack and animation
    private IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(1f / 12f * 5f);
        Instantiate(attackPrefab, attackFirepoint.position, attackFirepoint.rotation);
        yield return new WaitForSeconds(1 / 12f * 3f);
        ToggleMovement();
    }

    // Constantly flips the sprite and also the point of attac
    private void Update() {
        attackFirepoint.localPosition = new Vector2(
            flipSprite
            ? -Math.Abs(attackFirepoint.localPosition.x)
            : Math.Abs(attackFirepoint.localPosition.x),
            attackFirepoint.localPosition.y
        );
        attackFirepoint.localEulerAngles = new Vector3(0, 0, flipSprite ? 180 : 0);
        spriteRenderer.flipX = flipSprite;
    }
}
