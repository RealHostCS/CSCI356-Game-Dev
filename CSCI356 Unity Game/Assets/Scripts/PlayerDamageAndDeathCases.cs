using UnityEngine;

public class PlayerDamageAndDeathCases : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenUI; // Assign your Death Screen Canvas in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            ShowDeathScreen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            ShowDeathScreen();
        }
    }

    private void ShowDeathScreen()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }
}
