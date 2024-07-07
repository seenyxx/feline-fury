using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int mainScene = 2;
    public AudioSource GameManagerAudio;
    public Slider gameVolumeSlider;
    public TextMeshProUGUI gameVolumeText;
    public Image background;
    public GameObject blurredBg;


    [Header("Menus")]
    public GameObject settingsMenu;
    public GameObject helpMenu;
    public GameObject creditsMenu;

    private bool showOtherMenu = false;
    private Color originalBgColour;
    public Color darkenBgColour;

    [Header("Difficulty Selection")]
    public GameManager gameManager;
    public Button easy;
    public Button normal;
    public Button hard;

    // Difficulty text to display
    private readonly string[] difficultyTips = {
        "All enemies have 20% reduced health and damage.",
        "Enemies are all at their normal health and damage. It is how the game is intended to be played.",
        "All enemies have 20% increased health and damage, good luck."
    };

    // Stats multipliers for each difficulty
    private readonly float[] difficultyEnemyMultis = {
        0.8f,
        1f,
        1.2f,
    };

    public TextMeshProUGUI difficultyTipsText;

    // Load main game scene
    public void LoadMain()
    {
        GameManagerAudio.mute = false;
        SceneManager.LoadScene(mainScene);
    }

    // Load the difficulty that has been selected
    private void LoadDifficulty()
    {
        int currentDifficulty = PlayerPrefs.GetInt("difficulty", 1);
        UpdateDifficultyButtons(currentDifficulty);
    }

    // Updates the difficulty buttons accordingly based on which one was selected
    private void UpdateDifficultyButtons(int difficulty)
    {
        List<Button> buttons = new()
        {
            easy,
            normal,
            hard
        };

        gameManager.SetEnemyMultis(difficultyEnemyMultis[difficulty]);
        difficultyTipsText.text = difficultyTips[difficulty];
        
        buttons[difficulty].interactable = false;
        buttons.RemoveAt(difficulty);

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    // Sets the difficulty and updates the UI
    private void SetDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        UpdateDifficultyButtons(difficulty);
    }

    // Public accessible methods to set difficulty
    // Easy
    public void EasyDifficulty()
    {
        SetDifficulty(0);
    }

    // Normal
    public void NormalDifficulty()
    {
        SetDifficulty(1);
    }

    // Hard
    public void HardDifficulty()
    {
        SetDifficulty(2);
    }

    // Load the difficulty stored in playerprefs on startup
    private void Start() {
        LoadDifficulty();
        GameManagerAudio.mute = true;
        float currentVol = PlayerPrefs.GetFloat("volume", 1f);
        
        AudioListener.volume = currentVol;
        gameVolumeText.text = $"{currentVol * 100}%";
        gameVolumeSlider.value = currentVol;

        originalBgColour = background.color;
    }

    // Toggle any menu
    public void ToggleMenu(GameObject menu)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (showOtherMenu)
        {
            gameObject.SetActive(true);
            menu.SetActive(false);
            background.color = originalBgColour;
            blurredBg.SetActive(false);
            showOtherMenu = false;
        }
        else
        {
            gameObject.SetActive(false);
            menu.SetActive(true);
            background.color = darkenBgColour;
            blurredBg.SetActive(true);
            showOtherMenu = true;
        }
    }

    // Show or hide the settings menu
    public void ToggleSettings()
    {
        ToggleMenu(settingsMenu);
    }

    public void ToggleHelp()
    {
        ToggleMenu(helpMenu);
    }

    public void ToggleCredits()
    {
        ToggleMenu(creditsMenu);
    }
    

    // Update the volume text to match the slider
    public void UpdateVolume()
    {
        float newVol = Mathf.Round(gameVolumeSlider.value * 100) / 100;
        gameVolumeText.text = $"{newVol * 100}%";
        AudioListener.volume = newVol;
        PlayerPrefs.SetFloat("volume", newVol);
    }

    // Quit game
    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
