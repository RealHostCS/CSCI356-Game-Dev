using UnityEngine;
using UnityEngine.SceneManagement;

public class Dying : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathScreenUI; // Assign in inspector
    public GameObject sprintBar;     // Assign in inspector

    private void Start()
    {
        // Hide death screen at start
        if (deathScreenUI != null)
            deathScreenUI.SetActive(false);

        if (sprintBar != null)
            sprintBar.SetActive(true);
    }

    private void OnEnable()
    {
        // Listen for scene load to refresh references
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-find UI objects in the newly loaded scene if they are missing
        if (deathScreenUI == null)
            deathScreenUI = GameObject.Find("DeathScreenUI"); // Use exact name in scene

        if (sprintBar == null)
            sprintBar = GameObject.Find("StaminaBar"); // Use exact name in scene

        // Ensure UI is hidden after reload
        if (deathScreenUI != null)
            deathScreenUI.SetActive(false);

        if (sprintBar != null)
            sprintBar.SetActive(true);
    }

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
            deathScreenUI.SetActive(true);

        if (sprintBar != null)
            sprintBar.SetActive(false);

        // Stop game
        Time.timeScale = 0f;

        // Unlock cursor for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
