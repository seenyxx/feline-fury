using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 50f;
    public float currentHealth;
    public GameObject deathEffect;

    [Header("Display")]
    public bool displayDamageText = true;
    public GameObject damageText;
    public GameObject criticalDamageText;

    [Header("Management")]
    public string enemyManagerId;
    private EnemyManager enemyManager;
    
    [Header("Accumulated Damage Text")]
    public float accumDamage = 0f;
    private GameObject lastDmgText;


    void Start()
    {
        // Acquire enemy manager in order to determine the difficulty level of hte enemies
        GameObject enemyManagerObj = GameObject.Find("EnemyManager");

        if (enemyManagerObj) enemyManager = enemyManagerObj.GetComponent<EnemyManager>();

        if (enemyManagerObj)
            currentHealth = enemyManager.gm.enemyHealthMultiplier * maxHealth;
        else
            currentHealth = maxHealth;
    }

    // Adds spaces between each character in a string
    private string PadSpacesToText(string text)
    {
        return string.Join(" ", text.ToCharArray());
        /** 
        Space padding in between each character is needed because the Monsterrat font in Unity is really 
        glitched for no reason and has issues where it randomly just creates a 
        newline with certain numbers or letter combinations 
        **/
    }

    // Sbutract health method
    public void SubtractHealth(float amount, bool crit = false)
    {
        if (!enabled) return;
        currentHealth -= amount;

        if (displayDamageText) CreateDamageText(amount, crit);

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    // Creates the damage text
    private void CreateDamageText(float amount, bool crit)
    {
        if (crit)
        {
            GameObject critText = Instantiate(criticalDamageText, transform.position, Quaternion.identity);
            critText.GetComponent<TextMeshPro>().text = PadSpacesToText($"-{amount}");
        }
        else
        {
            if (lastDmgText && lastDmgText.activeInHierarchy)
            {
                accumDamage += amount;
                Destroy(lastDmgText);
            }
            else
            {
                accumDamage = amount;
            }

            lastDmgText = Instantiate(damageText, transform.position, Quaternion.identity);
            lastDmgText.GetComponent<TextMeshPro>().text = PadSpacesToText($"-{accumDamage}");
        }
    }


    // Processes to happen prior to the enemy death
    private void TriggerDeath()
    {
        // Updates the enemy manager object with accurate information of enemies
        if (enemyManager)
            enemyManager.DelayDecrementEnemyCount(enemyManagerId);
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        EnemyDrops enemyDrops = GetComponent<EnemyDrops>();

        if (enemyDrops)
        {
            enemyDrops.DropItems();
        }

        Destroy(gameObject);
    }
}
