using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Playlists")]
    public AudioClip[] menuTracks;
    public AudioClip[] selectTracks;
    public AudioClip[] gameTracks;

    [Header("Settings")]
    public float crossfadeTime = 1f;
    public float pausedVolumeMultiplier = 0.3f; // volume when paused

    [Header("Select Trigger")]
    public bool isSelect = false;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource activeSource;
    private bool isPaused = false;

    void Awake()
    {
        // Singleton + persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();
        sourceA.loop = true;
        sourceB.loop = true;
        activeSource = sourceA;

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Choose playlist based on scene name
        if (newScene.name.Contains("MainMenu") && !isSelect)
        {
            PlayRandomFrom(menuTracks);
        }
        else if (newScene.name.Contains("MainMenu") && isSelect)
        {
            PlayRandomFrom(selectTracks);
        }
        else
        {
            // Assume gameplay scenes
            PlayRandomFrom(gameTracks);
        }
    }

    public void PlaySelectMusic()
    {
        if (isSelect)
        {
            PlayRandomFrom(selectTracks);
        }
        else
        {
            PlayRandomFrom(menuTracks);
        }
    }

    private void PlayRandomFrom(AudioClip[] playlist)
    {
        if (playlist == null || playlist.Length == 0) return;

        AudioClip clip = playlist[Random.Range(0, playlist.Length)];
        StartCoroutine(CrossfadeTo(clip));
    }

    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        if (newClip == null) yield break;

        AudioSource newSource = (activeSource == sourceA) ? sourceB : sourceA;
        AudioSource oldSource = activeSource;

        newSource.clip = newClip;
        newSource.volume = isPaused ? pausedVolumeMultiplier : 1f;
        newSource.Play();

        float t = 0f;
        while (t < crossfadeTime)
        {
            t += Time.unscaledDeltaTime;
            float k = t / crossfadeTime;

            if (oldSource.isPlaying)
                oldSource.volume = Mathf.Lerp(isPaused ? pausedVolumeMultiplier : 1f, 0f, k);
            newSource.volume = Mathf.Lerp(0f, isPaused ? pausedVolumeMultiplier : 1f, k);

            yield return null;
        }

        if (oldSource.isPlaying)
            oldSource.Stop();

        activeSource = newSource;
    }

    // Call this when game is paused/unpaused
    public void SetPaused(bool paused)
    {
        isPaused = paused;
        float targetVolume = paused ? pausedVolumeMultiplier : 1f;

        sourceA.volume = targetVolume;
        sourceB.volume = targetVolume;
    }
}
