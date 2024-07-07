using System.Collections;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private WaitForSecondsRealtime _waitForSecondsRealtime;
    public float fpsRefreshTime = 1f;
    public int fpsTarget = 150;


    // Displays FPS every x amount of seconds
    private IEnumerator Start()
    {
        Application.targetFrameRate = fpsTarget;
        SetWaitForSecondsRealtime();
        while (true)
        {
            fpsText.text = $"{(int)(1 / Time.unscaledDeltaTime)} FPS";
            yield return _waitForSecondsRealtime;
        }
    }

    // Method for setting the wait time variable
    private void SetWaitForSecondsRealtime()
    {
        _waitForSecondsRealtime = new WaitForSecondsRealtime(fpsRefreshTime);
    }
}
