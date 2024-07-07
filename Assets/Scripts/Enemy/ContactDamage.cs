using UnityEngine;

public class ContactDamage : MonoBehaviour
{

    public float contactDamage = 5f;
    public int contactDamageInvincibilityFrames = 5;
    public float pushForce = 0f;
    public string playerTag = "PlayerEntity";

    private PlayerHealth playerHealth;

    // Does damage on contact with player
    private void OnTriggerEnter2D(Collider2D other) {
        // Exits out of method if the contacted object is not the player
        if (!other.CompareTag(playerTag)) return;

        if (!playerHealth)
            playerHealth = other.GetComponent<PlayerHealth>();

        // Subtract health from player and initiate i-frames
        playerHealth.SubtractHealth(contactDamage);
        playerHealth.InitInvincibilityFrames(contactDamageInvincibilityFrames);
    }
}
