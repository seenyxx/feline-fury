using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Image _staminaBarFill;
    public float maxStamina = 100f;
    public float currentStamina;
    public bool staminaDisabled = false;

    [Header("Styling")]
    public Color staminaBarColor = Color.white;
    public Color staminaDisabledColor = Color.gray;

    [Header("Stamina Regen")]
    public float staminaDecreasePerFrame = 1.5f;
    public float staminaIncreasePerFrame = 3f;
    public float staminaTimeToRegen = 3f;

    private float staminaRegenTimer = 0f;

    [Header("Stamina Orbs")]
    public float orbStaminaGain = 5f;
    public float orbManaGain = 2f;
    public string staminaOrbTag = "StaminaOrbs";
    public GameObject orbAbsorbParticle;
    public PlayerMana manaController;

    // Start is called before the first frame update
    void Start()
    {
        _staminaBarFill.color = staminaBarColor;
        currentStamina = maxStamina;
        _staminaBarFill.fillAmount = currentStamina / maxStamina;
    }

    // Subtract stamina and also reset the stamina regen timer
    public void SubtractStamina(float amount)
    {
        currentStamina -= amount;
        staminaRegenTimer = 0f;

        if (currentStamina <= 0)
        {
            staminaDisabled = true;
            _staminaBarFill.color = staminaDisabledColor;
        }
    }

    // Add stamina and also update the colour of the stamina bar
    public void AddStamina(float amount)
    {
        currentStamina += amount;

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            staminaDisabled = false;
            _staminaBarFill.color = staminaBarColor;
        }
    }

    // Constantly updating the stamina bar andalso regenning
    void Update()
    {
        // Constantly update stamina bar because the stamina is always regenning and changing
        _staminaBarFill.fillAmount = currentStamina / maxStamina;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Sprinting and stamina regeneration after a set period of time
        if (isSprinting && !staminaDisabled)
        {
            currentStamina = Mathf.Clamp(currentStamina - (staminaDecreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            staminaRegenTimer = 0f;

            if (currentStamina == 0)
            {
                staminaDisabled = true;
                _staminaBarFill.color = staminaDisabledColor;
            }
        }
        else if (currentStamina < maxStamina)
        {
            if (staminaRegenTimer >= staminaTimeToRegen)
            {
                currentStamina = Mathf.Clamp(currentStamina + (staminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;
            }
        }
        else if (currentStamina == maxStamina)
        {
            _staminaBarFill.color = staminaBarColor;
            staminaDisabled = false;
        }
    }

    // Check if player collides wtih stamina orb
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(staminaOrbTag))
        {
            manaController.AddMana(orbManaGain);
            AddStamina(orbStaminaGain);
            Instantiate(orbAbsorbParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
