using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("UI References")]
    public GameObject PauseScreen;
    public GameObject StaminaBar;
    public GameObject[] additionalUI; // Any other UI panels you want to manage

    [Header("Other References")]
    public GameObject freezeCam; // Camera object that has the mouse look script

    private bool isPaused = false;
    private MonoBehaviour mouseLookScript; // Reference to the script controlling mouse look

    void Start()
    {
        // Cache the first MonoBehaviour attached to freezeCam (or assign in inspector)
        if (freezeCam != null)
        {
            // Grab whichever script you’ve placed there (e.g. MouseLook)
            mouseLookScript = freezeCam.GetComponent<MonoBehaviour>();
        }

        // Hide all UI elements on start
        if (PauseScreen != null)
            PauseScreen.SetActive(false);

        if (StaminaBar != null)
            StaminaBar.SetActive(true);

        if (additionalUI != null)
        {
            foreach (GameObject ui in additionalUI)
            {
                if (ui != null)
                    ui.SetActive(false);
            }
        }

        Time.timeScale = 1f; // Ensure game runs normally
    }

    void Update()
    {
        // Toggle pause with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        // Disable mouse look instead of disabling camera
        if (mouseLookScript != null)
            mouseLookScript.enabled = false;

        if (StaminaBar != null)
            StaminaBar.SetActive(false);

        if (PauseScreen != null)
            PauseScreen.SetActive(true);

        Time.timeScale = 0f; // Stop game
        isPaused = true;

        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        // Re-enable mouse look
        if (mouseLookScript != null)
            mouseLookScript.enabled = true;

        if (PauseScreen != null)
            PauseScreen.SetActive(false);

        if (StaminaBar != null)
            StaminaBar.SetActive(true);

        // Hide all additional UI panels
        if (additionalUI != null)
        {
            foreach (GameObject ui in additionalUI)
            {
                if (ui != null)
                    ui.SetActive(false);
            }
        }

        Time.timeScale = 1f; // Resume game
        isPaused = false;

        // Lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // UNIVERSAL method to open any UI object
    public void OpenUI(GameObject uiToOpen)
    {
        if (uiToOpen != null)
        {
            // Hide main pause screen and all other UI panels
            if (PauseScreen != null)
                PauseScreen.SetActive(false);

            if (additionalUI != null)
            {
                foreach (GameObject ui in additionalUI)
                {
                    if (ui != null)
                        ui.SetActive(false);
                }
            }

            // Show the chosen UI
            uiToOpen.SetActive(true);
        }
    }
}
