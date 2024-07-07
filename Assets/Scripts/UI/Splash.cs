using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Splash : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public int mainMenuScene = 1;

    // Script that plays the splash screen video at the very beginning
    private void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(StartPlaying());
    }

    // Action to be performed upon the end of the video
    private void EndReached(VideoPlayer source)
    {
        source.playbackSpeed /= 10f;

        videoPlayer.Stop();
        SceneManager.LoadScene(mainMenuScene);
    }

    // Start playing the video
    IEnumerator StartPlaying()
    {
        yield return new WaitForSeconds(2);

        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }
}
