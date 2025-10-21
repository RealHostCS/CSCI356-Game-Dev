using UnityEngine;

public class IceScript : MonoBehaviour
{
    AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            collider.GetComponent<PlayerMovement>().SlipperyFriction();
            sound.Play();
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.name == "Player")
        {
            collider.GetComponent<PlayerMovement>().ResetFriction();
            sound.Stop();
        }
    }
}
