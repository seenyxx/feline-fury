using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipText : MonoBehaviour
{
    // List of tips to display
    private readonly List<string> tips = new()
    {
        "Use your dash to dodge enemy projectiles or to move around quickly",
        "Ghosts and their projectiles can travel through objects",
        "Pay attention to your stamina usage",
        "Blue orbs grant you stamina and a bit of mana",
        "If you fully deplete your stamina, you will not able to use it until it fully regenerates",
        "Be wary of your mana usage",
        "Some weapons use more mana than others",
        "Enemies can drop potions that can increase your health or mana",
        "If you are out of mana, your health will be consumed instead",
        "Interact with the arms dealer to purchase weapons, there will be one at the start and at the end of the dungeon",
        "Interact with little robots you find along the way to purchase potions"
    };

    private TextMeshProUGUI textComponent;
    private string selectedText;
    public float typingSpeedCpm = 3000f;

    // Picks out a string from a random list of tips to display at the loading screen
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = "";

        int randIndex = Random.Range(0, tips.Count);
        selectedText = tips[randIndex];

        StartCoroutine(TypeText());
    }

    // Types the text character by character instead of displaying it all at once
    private IEnumerator TypeText()
    {
        for (int i = 0; i < selectedText.Length; i++)
        {
            textComponent.text += selectedText[i];

            yield return new WaitForSeconds(60f / typingSpeedCpm);
        }
    }
}
