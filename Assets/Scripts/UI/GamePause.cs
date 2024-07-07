using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GamePause : MonoBehaviour
{
    public static bool isPaused = false;
    public Canvas pauseCanvas;
    public int mainMenuScene = 1;

    [Header("Player")]
    public Stamina playerStamina;
    public PlayerMana playerMana;
    public PlayerHealth playerHealth;
    public PlayerCoins playerCoins;
    public PlayerMovement playerMovement;
    public WeaponAim weaponAim;

    public WeaponPickupHandler weaponPickupHandler;
    public PlayerConsumableMagnet playerConsumableMagnet;
    public PotionHandler potionHandler;
    
    // Pause the game execution and also show the pause screen
    public void PauseGame()
    {
        pauseCanvas.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        SetActiveState(false);
    }

    // Unpause the game and hide the pause screen
    public void UnPauseGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        SetActiveState(true);
    }

    // Restart the currnet scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit to the main menu
    public void ExitMainMenu()
    {
        // Destroy(playerStamina.gameObject);
        // Destroy(gameObject);
        SceneManager.LoadScene(mainMenuScene);
    }

    // Constantly checking for input 
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Enabling or disabling player scripts
    public void SetActiveState(bool state)
    {
        playerStamina.enabled = state;
        playerMana.enabled = state;
        playerHealth.enabled = state;
        playerCoins.enabled = state;
        playerMovement.enabled  = state;
        weaponAim.enabled = state;
        weaponPickupHandler.enabled = state;
        playerConsumableMagnet.enabled = state;
        potionHandler.enabled = state;
    }
}
