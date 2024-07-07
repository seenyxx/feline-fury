using UnityEngine;
public class PreserveInGame : MonoBehaviour
{

    public bool originalState = true;
    public bool firstLoad;

    public string[] deletionScenes = {"Main", "Menu"};

    // Preserve the game object in game even when switching scenes
    private void Awake() {
        firstLoad = true;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(originalState);

        GameObject.Find("PreservationController").GetComponent<PreservationController>().preservedObjects.Add(this);
    }
}
