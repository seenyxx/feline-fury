using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Game manager script
public class GameManager : MonoBehaviour
{
    public float enemyHealthMultiplier = 1f;
    public float enemyDamageMultipliier = 1f;

    // Music
    public AudioSource musicSource;
    public AudioClip[] music;
    private AudioClip[] shuffledMusic;
    private int currentIndex;

    // Stopwatch
    private DateTime startTime;
    private TimeSpan elapsedTime = TimeSpan.Zero;

    public bool firstLoad = true;


    // Apply enemy multipliers
    public void SetEnemyMultis(float multi)
    {
        enemyHealthMultiplier = multi;
        enemyDamageMultipliier = multi;
    }

    // Preserve game object
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        DontDestroyOnLoad(gameObject);
    }

    // Destroy game object if scene is switched back to main menu
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (firstLoad)
        {
            firstLoad = false;
            return;
        }

        if (scene.name == "Main")
        {
            StartStopWatch();
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start the stop watch for the game
    private void StartStopWatch()
    {
        startTime = DateTime.Now;
    }

    // Stop the stop watch for the game
    public void StopStopWatch()
    {
        if (startTime != null)
        {
            elapsedTime += DateTime.Now - startTime;
        }
    }

    // Display the time
    public string DisplayTime()
    {
        int minutes = elapsedTime.Minutes;
        int seconds = elapsedTime.Seconds;
        int milliseconds = elapsedTime.Milliseconds;

        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    // Shuffle the music and play
    private void Start() {
        ShuffleClips();
    }

    // Check if music is not playing and if it isn't then play next track
    private void FixedUpdate() {
        if (!musicSource.isPlaying && Application.isFocused)
        {
            PlayNextClip();
        }
    }

    // Method to shuffle the music tracks
    private void ShuffleClips()
    {
        shuffledMusic = (AudioClip[])music.Clone();
        for (int i = 0; i < shuffledMusic.Length; i++)
        {
            AudioClip temp = shuffledMusic[i];
            int randomIndex = UnityEngine.Random.Range(0, shuffledMusic.Length);
            shuffledMusic[i] = shuffledMusic[randomIndex];
            shuffledMusic[randomIndex] = temp;
        }
    }

    // Play next music track in list
    private void PlayNextClip()
    {
        if (currentIndex >= shuffledMusic.Length)
        {
            currentIndex = 0;
            ShuffleClips();
        }

        musicSource.clip = shuffledMusic[currentIndex];
        musicSource.Play();
        currentIndex++;
    }
}
