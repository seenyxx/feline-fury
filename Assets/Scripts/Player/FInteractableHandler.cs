using System.Collections;
using TMPro;
using UnityEngine;

public class FInteractableHandler : MonoBehaviour
{
    public PlayerCoins playerCoins;
    public float interactRadius = 2f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI interactText;

    private bool statePaused = false;

    // Constantly check for interactable objects in a radius
    private void Update() {
        if (statePaused) return;

        Collider2D interactable = Physics2D.OverlapCircle(transform.position, interactRadius, interactableLayer);

        if (interactable)
        {
            InteractableShop interactableShop = interactable.GetComponent<InteractableShop>();

            if (!interactableShop) return;

            interactText.enabled = true;
            interactText.text = interactableShop.BeforeInteractionText(playerCoins.coins);

            // Check for keyboard input if there is an interactable object nearby
            if (Input.GetKeyDown(KeyCode.F))
            {
                int change = interactableShop.Interact(playerCoins.coins);

                playerCoins.coins += change;

                playerCoins.UpdateText();

                StartCoroutine(TempPause());

                if (change != 0)
                {
                    interactText.text = interactableShop.SuccessText();
                }
                else
                {
                    interactText.text = interactableShop.ErrorText();
                }
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    // Temporary pause in execution of the script
    private IEnumerator TempPause()
    {
        statePaused = true;
        yield return new WaitForSeconds(1f);
        statePaused = false;
    }
}
