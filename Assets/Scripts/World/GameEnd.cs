using System.Collections;
using UnityEngine;

public class GameEnd : MonoBehaviour {
    public LoadEndScreen loadEndScreen;
    public GameManager gm;

    // Find the load end screen object script when started
    private void Start() {
        if (!loadEndScreen)
        {
            loadEndScreen = GameObject.Find("EndScreenManager").GetComponent<LoadEndScreen>();
        }

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(DelayEnding());
    }

    // Delay the appearance of the end screen
    private IEnumerator DelayEnding()
    {
        gm.StopStopWatch();
        yield return new WaitForSeconds(2f);
        loadEndScreen.EndGame(gm.DisplayTime());
    }
}