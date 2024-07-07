using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerDeath playerDeath;
    public Image _healthBarFill;
    public float currentHealth = 0f;
    public float maxHealth = 100f;
    public float fillUpdateSpeed = 1f;
    public TextMeshProUGUI _healthText;
    public int invincibilityTicks = 5;
    private int currentMaxITicks = 0;

    private float lerpTime = 0f;
    private int currentITicks = 0;
    public bool isInvincible = false;

    [Header("Renderer")]
    public SpriteRenderer playerSprite;
    public Material normalMaterial;
    public Material damagedMaterial;
    public float damageFlashDuration = 0.1f;

    [Header("HealthPots")]
    public ParticleSystem healthPickupParticle;
    public GameObject healText;


    // Sets the current health to the maximum health at the start
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Custom method to subtract health that allows for smooth transition at the health bar
    public void SubtractHealth(float amount) {
        if (isInvincible) return;
        float healthLeft = currentHealth - amount;

        if (healthLeft > 0) {
            currentHealth -= amount;
        }
        else
        {
            currentHealth = 0;
            // Trigger death

            playerDeath.TriggerDeath();
        }

        UpdateText();
        SpriteDamageEffect();
        lerpTime = 0f;

        StartInvincibility();
    }

    // Change the material of the player to glow red when damaged
    private void SpriteDamageEffect()
    {
        playerSprite.material = damagedMaterial;
        StartCoroutine(ResetPlayerSpriteMaterial());
    }

    // Resets the material back to default

    private IEnumerator ResetPlayerSpriteMaterial()
    {
        yield return new WaitForSeconds(damageFlashDuration);
        playerSprite.material = normalMaterial;
    }

    // Custom adding health method that allows for smooth transition of the health bar
    public void AddHealth(float amount) {
        float futureHealth = currentHealth + amount;

        if (futureHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }

        UpdateText();

        lerpTime = 0f;
    }

    // Updates the UI element, is triggered every single time the health changes
    private void UpdateHealthBar()
    {
        lerpTime += Time.deltaTime * fillUpdateSpeed;
        float start = _healthBarFill.fillAmount;
        float target = currentHealth / maxHealth;
        _healthBarFill.fillAmount = Mathf.Lerp(start, target, lerpTime);
    }

    // Updates the health text 
    private void UpdateText()
    {
        _healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // Checks for invincibility frames and if there is, then the player is made invincible
    private void FixedUpdate()
    {
        // I-Frame checker
        if (isInvincible)
        {
            currentITicks++;

            if (currentITicks >= currentMaxITicks)
            {
                isInvincible = false;
            }
        }
    }

    // Constantly updating the health bar
    private void Update() {
        UpdateHealthBar();
    }

    // I-Frame implementation

    private void StartInvincibility()
    {
        currentITicks = 0;
        currentMaxITicks = invincibilityTicks;
        isInvincible = true;
    }

    // I-Frames for dashes, dodges and damage
    public void InitInvincibilityFrames(int frames)
    {
        currentITicks = 0;
        currentMaxITicks = frames;
        isInvincible = true;
    }

    // Adds health after consumption of health potion
    public void ConsumeHealthPotion(float amount)
    {
        healthPickupParticle.Play();
        var healDisplay = Instantiate(healText, transform.position, Quaternion.identity);
        healDisplay.GetComponent<TextMeshPro>().text = $"+ {amount}";

        AddHealth(amount);
    }
}
