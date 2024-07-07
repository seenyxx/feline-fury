using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDrops : MonoBehaviour
{
    [Header("Stamina Orbs")]
    public int minStaminaOrbs = 0;
    public int maxStaminaOrbs = 2;
    public GameObject staminaOrb;
    public float staminaOrbSpawnRadius = 0.8f;

    [Header("Coins")]
    public int minCoins = 0;
    public int maxCoins = 2;
    public GameObject coinPrefab;
    public float coinSpawnRadius = 0.8f;

    [Header("Potions")]
    public bool dropPots = true;
    public float potDropPercentage = 30f;
    public float largePotChancePercentage = 35f;
    public float tunaDropPercentage = 10f;
    public GameObject tunaPrefab;

    [Header("Health Potions")]
    public GameObject miniHealthPotPrefab;
    public GameObject largeHealthPotPrefab;

    [Header("Mana Potions")]
    public GameObject miniManaPotPrefab;
    public GameObject largeManaPotPrefab;

    // Evaluate the outcome of a percentage probability
    private bool EvaluateChance(float chance)
    {
        float random = Random.Range(0f, 100f);
        return random <= chance;
    }

    // Drop items after the death of the enemy
    public void DropItems()
    {
        SpecialEnemyDrops specialEnemyDrops = GetComponent<SpecialEnemyDrops>();
        if (specialEnemyDrops)
        {
            foreach (SpecialDrop specialDrop in specialEnemyDrops.specialDrops)
            {
                if (EvaluateChance(specialDrop.chance))
                {
                    Instantiate(specialDrop.obj, transform.position, Quaternion.identity);
                }
            }
        }
        // Drop stamina Orbs
        InstantiateRandom(staminaOrb, minStaminaOrbs, maxStaminaOrbs, staminaOrbSpawnRadius);
        InstantiateRandom(coinPrefab, minCoins, maxCoins, coinSpawnRadius);

        // Evaluate tuna drop percentage
        if (EvaluateChance(tunaDropPercentage))
        {
            Instantiate(tunaPrefab, transform.position, Quaternion.identity);
            return;
        }

        // Evaluate potion drop percentage
        if (EvaluateChance(potDropPercentage) && dropPots)
        {
            if (EvaluateChance(50f))
            {
                // Health potions
                if (EvaluateChance(largePotChancePercentage))
                {
                    // Large health pot
                    Instantiate(largeHealthPotPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    // Small health pot
                    Instantiate(miniHealthPotPrefab, transform.position, Quaternion.identity);
                }
            }
            else
            {
                // Mana potions
                if (EvaluateChance(largePotChancePercentage))
                {
                    // Large mana pot
                    Instantiate(largeManaPotPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    // Small mana pot
                    Instantiate(miniManaPotPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    // Custom random function 
    private int GetRandomCount(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    // Method to generate random amounts of objects within an area
    private void InstantiateRandom(GameObject prefab, int min, int max, float radius)
    {
        int objCount = GetRandomCount(min, max);

        for (int i = 0; i < objCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);

            Vector2 randomPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            Vector3 spawnPos = (Vector3)randomPos + transform.position;

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}
