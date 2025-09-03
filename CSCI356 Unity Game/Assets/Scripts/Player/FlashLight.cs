using UnityEngine;

public class FlashLight : MonoBehaviour
{
    // Assign the flashlight GameObject in the Inspector
    public GameObject flashlight;

    //Audio Clips
    public AudioClip humSound;    // looping hum
    public AudioClip onSound;     // click when turning on
    public AudioClip offSound;    // click when turning off

    // Audio Sources
    public AudioSource humSource; // dedicated to looping hum
    public AudioSource sfxSource; // dedicated to one-shot SFX

    // Settings
    private bool isOn = true;
    void Start()
    {
        if (flashlight == null)
        {
            Debug.LogError("Flashlight GameObject not assigned in Inspector!");
        }

        // Ensure hum source is set up for looping the hum clip
        if (humSource != null)
        {
            humSource.loop = true;
            humSource.clip = humSound;
        }
        else
        {
            Debug.LogWarning("Hum AudioSource not assigned.");
        }

        // Ensure SFX source exists
        if (sfxSource == null)
        {
            Debug.LogWarning("SFX AudioSource not assigned.");
        }

        // Set initial flashlight state
        if (flashlight != null)
            flashlight.SetActive(isOn);

        // Start or stop hum based on initial state
        if (humSource != null)
        {
            if (isOn && humSource.clip != null)
                humSource.Play();
            else
                humSource.Stop();
        }
    }

    void Update()
    {
        // Left mouse button toggles light
        if (Input.GetMouseButtonDown(0))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;

        if (flashlight != null)
            flashlight.SetActive(isOn);

        if (isOn)
        {
            // Start hum (loop)
            if (humSource != null && humSource.clip != null && !humSource.isPlaying)
                humSource.Play();

            // Play ON click
            if (sfxSource != null && onSound != null)
                sfxSource.PlayOneShot(onSound);
        }
        else
        {
            // Stop hum
            if (humSource != null && humSource.isPlaying)
                humSource.Stop();

            // Play OFF click
            if (sfxSource != null && offSound != null)
                sfxSource.PlayOneShot(offSound);
        }
    }
}
