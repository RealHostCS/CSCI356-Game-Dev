using UnityEngine;
using System.Collections;

public class CameraItem : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip onSound;
    public Light flashLight;
    public float flashDuration = 0.2f;
    public float cooldown = 20f;
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
        if (flashLight != null) flashLight.enabled = true;
        if (sfxSource != null && onSound != null) sfxSource.PlayOneShot(onSound);
        yield return new WaitForSeconds(flashDuration);
        if (flashLight != null) flashLight.enabled = false;

        // Raycast from the light forward
        var origin = flashLight != null ? flashLight.transform.position : transform.position;
        var dir    = flashLight != null ? flashLight.transform.forward  : transform.forward;

        Debug.DrawRay(origin, dir * rayDistance, Color.white, 0.25f);
        Debug.Log("Sending raycast");

        if (Physics.Raycast(origin, dir, out RaycastHit hit, rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            var monster = hit.collider.GetComponent<MonsterStates>() ?? hit.collider.GetComponentInParent<MonsterStates>();
            if (monster != null)
            {
                Debug.Log("Monster hit by flash!");
                monster.Stun(3f);
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
