using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEndScreen : MonoBehaviour {
    private GameObject player;
    private PlayerCoins playerCoins;
    public TextMeshProUGUI coinsCounter;
    public TextMeshProUGUI timerText;
    public int mainMenuScene = 1;
    public Canvas endScreen;

    // Disable the end screen before starting
    private void Awake() {
        endScreen.enabled = false;
    }

    // Acquire the player and player coins
    private void Start()
    {
        player = GameObject.Find("Player");

        playerCoins = player.GetComponent<PlayerCoins>();
    }

    // End the game and show endscreen
    public void EndGame(string timerDisplay)
    {
        coinsCounter.text = playerCoins.coins.ToString();
        timerText.text = timerDisplay;
        endScreen.enabled = true;
        Destroy(player);
    }

    // Exit to main menu
    public void ExitMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}