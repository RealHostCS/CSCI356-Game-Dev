using UnityEngine;
using System.Collections;

public class CameraItem : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip onSound;
    public Light flashLight;
    public float flashDuration = 0.2f;
    public float cooldown = 5f;
    public float rayDistance = 30f;

    private bool isCharging = false;

    void Start()
    {
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
        if (sfxSource != null && onSound != null)
            sfxSource.PlayOneShot(onSound);
        yield return new WaitForSeconds(flashDuration);
        flashLight.enabled = false;

        // Use ray cast to check if monster in in light
        var origin = flashLight != null ? flashLight.transform.position : transform.position;
        var dir    = flashLight != null ? flashLight.transform.forward  : transform.forward;
        Ray ray = new Ray(origin, dir);
        Debug.Log("Sending raycast");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Monster"))
            {
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

    private void OnEnable()
    {
        isCharging = false;
    }
}
