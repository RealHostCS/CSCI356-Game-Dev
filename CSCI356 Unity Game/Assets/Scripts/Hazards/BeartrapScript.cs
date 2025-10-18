using UnityEngine;
using System.Collections;

public class BeartrapScript : MonoBehaviour
{
    private Animator animator;
    private AudioSource sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            animator.Play("BeartrapClose");
            sound.Play();
            collider.GetComponent<TakeDamage>().StartDamage(10);
            collider.GetComponent<PlayerMovement>().StartSlow(0.0f);
            StartCoroutine(HoldInPlace(collider));
        }
    }

    private IEnumerator HoldInPlace(Collider collider)
    {
        yield return new WaitForSeconds(2.0f);
        collider.GetComponent<PlayerMovement>().ResetSpeed();
        yield break;
    }
}
