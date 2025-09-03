using UnityEngine;
using System.Collections;

public class CameraItem : MonoBehaviour
{
    //public AudioSource sfxSource;
    //public AudioClip onSound;
    public Light flashLight;           // Assign a Light in the Inspector
    public float flashDuration = 0.2f; // How long the flash lasts
    public float cooldown = 5f;        // Cooldown between flashes
    public float rayDistance = 30f;

    private bool isCharging = false;

    void Start()
    {
        /* Ensure SFX source exists
        if (sfxSource == null)
        {
            Debug.LogWarning("SFX AudioSource not assigned.");
        }*/
        if (flashLight == null)
        {
            Debug.LogError("Flash light is not assigned in the inspector!");
        }
        else
        {
            flashLight.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCharging)
        {
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        isCharging = true;

        // Flash the light
        flashLight.enabled = true;
        // Play ON sound
        /*if (sfxSource != null && onSound != null)
        {
            sfxSource.PlayOneShot(onSound);           
        }*/
        yield return new WaitForSeconds(flashDuration);
        flashLight.enabled = false;

        // Use ray cast to check if monster in in light
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.Log("Sending raycast");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Monster"))
            {
                // Example: tell the monster script to change state
                MonsterStats monster = hit.collider.GetComponent<MonsterStats>();
                if (monster != null)
                {
                    monster.Stun();
                }
            }
        }

        // Wait cooldown before allowing another flash
        yield return new WaitForSeconds(cooldown);
        isCharging = false;
    }
}
