using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public Image _manaBarFill;
    public PlayerHealth playerHealth;
    public float currentMana = 0f;
    public float maxMana = 250f;
    
    [Header("ManaPots")]
    public ParticleSystem manaPickupParticle;
    public GameObject manaHealText;

    // Sets the current mana to the maximum at the start
    private void Start() {
        currentMana = maxMana;
    }

    // Custom method to subtract mana while using a smooth bar transition
    public void SubtractMana(float amount)
    {
        float manaLeft = currentMana - amount;
        if (manaLeft > 0f)
        {
            currentMana = manaLeft;
        }
        else
        {
            // Subtracts 80% of usage from health if no more mana is left
            currentMana = 0f;
            playerHealth.SubtractHealth(Mathf.Round(amount * 0.8f));
        }

        UpdateManaBar();
    }

    public void AddMana(float amount)
    {
        float manaIncreased = currentMana + amount;
        currentMana = manaIncreased > maxMana ? maxMana : manaIncreased;
        
        UpdateManaBar();
    }

    // Updates mana on value update
    private void UpdateManaBar()
    {
        _manaBarFill.fillAmount = currentMana / maxMana;
    }

    // Adds mana after mana potion consumption

    public void ConsumeManaPotion(float amount)
    {
        manaPickupParticle.Play();
        var manaDisplay = Instantiate(manaHealText, transform.position, Quaternion.identity);
        manaDisplay.GetComponent<TextMeshPro>().text = $"+ {amount}";

        AddMana(amount);
    }
}
