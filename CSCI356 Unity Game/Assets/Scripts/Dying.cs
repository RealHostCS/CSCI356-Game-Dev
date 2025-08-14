using UnityEngine;

public class Dying : MonoBehaviour
{
    public GameObject deathScreenUI; // Assign in inspector

    private void Start()
    {
        // Hide the death screen at the start
        deathScreenUI.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collison");
        if (collision.gameObject.CompareTag("Monster"))
        {
            ShowDeathScreen();
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.CompareTag("Monster"))
        {
            ShowDeathScreen();
        }
    }


    private void ShowDeathScreen()
    {
        Debug.Log("yeah you got caught");
        deathScreenUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Lock time so game pauses
        Time.timeScale = 0f;

        // Unlock mouse so the player can click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
