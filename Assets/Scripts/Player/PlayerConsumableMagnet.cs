using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumableMagnet : MonoBehaviour
{
    public float magnetRadius = 3f;
    public float attractionSpeed = 5f;
    public float accelFactor = 2f;
    public LayerMask attractedConsumableLayer;

    [Header("Coins")]
    public string coinTag = "PlayerCoins";
    public float coinSpeedMulti = 0.5f;


    // Magnetises all coins and orbs towards the player every game tick
    private void FixedUpdate() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius, attractedConsumableLayer);

        // Loops through all the nearby coins and orbs and applies motion
        foreach (Collider2D col in colliders)
        {
            Transform colTransform = col.transform;
            Vector3 direction = transform.position - colTransform.position;

            float distance = direction.magnitude;
            float acceleration = 1 / distance * accelFactor;

            // Applies acceleration as well
            Vector3 positionChange = acceleration * attractionSpeed * Time.fixedDeltaTime * direction.normalized;
            
            if (col.gameObject.CompareTag(coinTag))
            {
                positionChange *= coinSpeedMulti;
            }

            colTransform.position += positionChange;
        }
    }
}
