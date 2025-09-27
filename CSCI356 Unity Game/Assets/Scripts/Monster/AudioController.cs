using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{

    public AudioSource audioSource;   // Assign in Inspector
    public AudioClip clip;            // The clip to play
    public float interval = 3f;       // Time in seconds between plays
    public bool playOnStart = true;   // Should it start automatically?

    private Coroutine playRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playOnStart)
        {
            StartPlaying();
        }
    }

    public void StartPlaying()
    {
        if (playRoutine == null && audioSource != null && clip != null)
        {
            playRoutine = StartCoroutine(PlayLoop());
        }
    }

    public void StopPlaying()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(interval);
        }
    }
}
