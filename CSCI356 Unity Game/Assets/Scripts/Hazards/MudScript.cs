using UnityEngine;

public class MudScript : MonoBehaviour
{
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            gameObject.GetComponent<PlayerMovement>().StartSlow(0.2f);
            sound.Play();
        }
    }

    public void OnTriggerExit(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            gameObject.GetComponent<PlayerMovement>().ResetSpeed();
            sound.Stop();
        }
    }
}
