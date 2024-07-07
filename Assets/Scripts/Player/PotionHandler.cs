using System.Collections;
using UnityEngine;

public class PotionHandler : MonoBehaviour
{
    [Header("Potions")]
    public PlayerHealth playerHealth;
    public Stamina playerStamina;
    public PlayerMana playerMana;
    public Transform potDespawnParticle;
    public string potionPickupTag = "PlayerInteractConsumables";
    
    // Delays the display of mana consumption when consuming tuna to avoid UI clutter
    private IEnumerator DelayManaConsumption(float amount)
    {
        yield return new WaitForSeconds(0.3f);
        playerMana.ConsumeManaPotion(amount);
    }

    // Handle all potion collision detection
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag(potionPickupTag))
        {
            ConsumableBuff consumableMeta = other.GetComponent<ConsumableBuff>();

            float healthAddition = consumableMeta.healthAddition;
            float manaAddition = consumableMeta.manaAddition;
            float staminaAddition = consumableMeta.staminaAddition;

            bool used = false;


            if (healthAddition > 0 && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                used = true;
                playerHealth.ConsumeHealthPotion(healthAddition);
            }
            
            if (manaAddition > 0 && playerMana.currentMana < playerMana.maxMana)
            {
                if (healthAddition > 0 && used)
                    StartCoroutine(DelayManaConsumption(manaAddition));
                else
                    playerMana.ConsumeManaPotion(manaAddition);

                used = true;
            }

            if (staminaAddition > 0 && used)
            {
                playerStamina.AddStamina(staminaAddition);
            }


            if (used)
            {
                // Only destroy potion if used
                Instantiate(potDespawnParticle, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }
}
