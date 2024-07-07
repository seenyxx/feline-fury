using UnityEngine.SceneManagement;

// Extends off the base class of interactable shop
public class EndPortal: InteractableShop
{
    // Text that pops up before the interaction
    public override string BeforeInteractionText(int coins) {
        return "<mark=#33333322>[F] Enter boss fight?</mark>";
    }

    // Error
    public override string ErrorText() {
        return "";
    }

    // Interact with portal to be transported to boss room
    public override int Interact(int coins)
    {
        SceneManager.LoadScene(3);
        return 0;
    }
}