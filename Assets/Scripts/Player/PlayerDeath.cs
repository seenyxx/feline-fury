using System;
using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public float slowMotionSpeed = 0.5f;
    public float slowMotionDuration = 1f;
    public CanvasGroup deathScreen;
    public float FadeRate;
    public GamePause gamePause;

    // Trigger the death of the player
    public void TriggerDeath()
    {
        deathScreen.gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }

        StartCoroutine(SlowMotion());
        StartCoroutine(FadeInDeathScreen());
    }

    // Makes the game slow motion right before the actual death
    private IEnumerator SlowMotion()
    {
        Time.timeScale = slowMotionSpeed;
        yield return new WaitForSeconds(slowMotionDuration);
        Time.timeScale = 0f;
    }

    // Fade the deathscreen in as the slow motion happens
    private IEnumerator FadeInDeathScreen()
    {
        float target = 1f;
        float currentAlpha = deathScreen.alpha;

        while(Mathf.Abs(currentAlpha - target) > 0.0001f)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, target, FadeRate * Time.deltaTime);
            
            deathScreen.alpha = currentAlpha;
            yield return null;
        }

        gamePause.SetActiveState(false);
    }
}
