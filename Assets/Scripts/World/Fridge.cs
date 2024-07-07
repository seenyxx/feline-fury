using UnityEngine;

// Extends off the base class of interactable shop
public class Fridge : InteractableShop
{

    [Header("Potion RNG")]
    public GameObject largeHealth;
    public GameObject smallHealth;
    public GameObject largeMana;
    public GameObject smallMana;
    public GameObject fishConsumable;

    // Probability of each potion
    private readonly float healthChance = 0.5f;
    private readonly float largeChance = 0.2f;
    private readonly float fishChance = 0.08f;

    // Sets metadata for display text
    private void Start()
    {
        displayText = "Buy a potion?";
    }

    // Action to perform on interaction
    public override int Interact(int coins)
    {
        if (coins >= cost)
        {
            DropPotion();
            return -cost;
        }
        else
        {
            return 0; 
        }
    }

    // Evaluate chances for potion drops
    private void DropPotion()
    {
        if (EvalChance(fishChance))
        {
            InstantiateObject(fishConsumable);
            return;
        }

        if (EvalChance(largeChance))
        {
            if (EvalChance(healthChance))
            {
                InstantiateObject(largeHealth);
            }
            else
            {
                InstantiateObject(largeMana);
            }
        }
        else
        {
            if (EvalChance(healthChance))
            {
                InstantiateObject(smallHealth);
            }
            else
            {
                InstantiateObject(smallMana);
            }
        }
    }
}
