using UnityEngine;

public class WinningDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenWinScreen();
        }

    }

    void OpenWinScreen()
    {

    }
}
