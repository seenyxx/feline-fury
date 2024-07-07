using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathText : MonoBehaviour
{
    private readonly List<string> deathMessages = new()
    {
        "You Died",
        "Unlucky",
        "Next time..",
        "Unfortunate",
        "Wasted"
    };
    public TextMeshProUGUI deathText;

    // Dispaly the death text
    private void Start() {
        int selectedText = Random.Range(0, deathMessages.Count);
        deathText.text = deathMessages[selectedText];
    }
}
