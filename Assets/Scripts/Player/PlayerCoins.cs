using TMPro;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public TextMeshProUGUI coinCounter;
    public int coins = 0;

    [Header("Coin properties")]
    public string coinTag = "PlayerCoins";
    public GameObject coinAbsorbParticle;

    // Adds a coin every time a coin touches the player
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(coinTag))
        {
            coins += 1;
            UpdateText();
            Instantiate(coinAbsorbParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    // Updates the coin counter text
    public void UpdateText()
    {
        coinCounter.text = coins.ToString();
    }
}
