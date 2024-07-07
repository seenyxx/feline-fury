using System;
using System.Collections.Generic;
using UnityEngine;

// Serialisable key value storage for weapon and their chances
[Serializable]
public class WeaponsChance
{
    public GameObject weaponPrefab;
    public float chance;
}

// Extends off the base class interactable shop
public class ArmsDealer : InteractableShop
{
    public List<WeaponsChance> weaponsChances;

    // Set metadata at the start
    private void Start() {
        displayText = "Buy a weapon?";
    }

    // Interact with arms dealer and exchange coins for weapons
    public override int Interact(int coins)
    {
        if (coins >= cost)
        {
            foreach (WeaponsChance weaponsChance in weaponsChances)
            {
                if (EvalChance(weaponsChance.chance))
                {
                    InstantiateObject(weaponsChance.weaponPrefab);
                    break;
                }
            }
            return -cost;
        }
        else
        {
            return 0;
        }
    }
}