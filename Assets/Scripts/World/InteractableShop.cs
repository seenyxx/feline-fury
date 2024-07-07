using UnityEngine;

// Interactable shop class that forms the backbone of interactable objects
public class InteractableShop: MonoBehaviour
{

    public int cost = 15;
    public int maxCost = 35;
    public int minCost = 15;
    public GameObject spawnEffect;
    public Transform spawnPt;

    // Randomise the cost
    private void Awake() {
        cost = Random.Range(minCost, maxCost + 1);
    }

    // Default display text
    protected string displayText = "Buy a potion?";

    // Default before interaction text
    public virtual string BeforeInteractionText(int coins)
    {
        return $"<mark=#33333322>[F] {displayText} <color=\"{(coins >= cost ? "yellow" : "red")}\">{cost} coins</color></mark>";
    }

    // Default error text
    public virtual string ErrorText()
    {
        return "<mark=#33333322>Not enough coins!</mark>";
    }

    // Default success text
    public virtual string SuccessText()
    {
        return "<mark=#33333322>Purchased!</mark>";
    }

    // Stub interact function that is made to be overwritten
    public virtual int Interact(int coins)
    {
        Debug.Log("Interact not implemented");
        return 0;
    }

    // Create object at spawn point location
    protected void InstantiateObject(GameObject obj)
    {
        Instantiate(spawnEffect, spawnPt.position, Quaternion.identity);
        Instantiate(obj, spawnPt.position, Quaternion.identity);
    }
    
    // Evaluate chance
    protected bool EvalChance(float chance)
    {
        float random = Random.value;
        return random <= chance;
    }
}
