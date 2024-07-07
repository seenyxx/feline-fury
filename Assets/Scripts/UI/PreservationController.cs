using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreservationController : MonoBehaviour {
    public List<PreserveInGame> preservedObjects;
    public GameObject destroyer;

    // Listen to scene change event
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    // Instantiates a destroyer component to destroy on load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject ds = Instantiate(destroyer, transform.position, Quaternion.identity);
        ds.GetComponent<Destroyer>().preservedObjects = preservedObjects;
    }
}