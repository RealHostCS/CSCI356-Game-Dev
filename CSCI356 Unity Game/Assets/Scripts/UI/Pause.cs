using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("UI References")]
    public GameObject PauseScreen;       // Main pause menu panel
    public GameObject StaminaBar;        // Stamina UI
    public GameObject[] additionalUI;    // Other UI panels (inventory, settings, etc.)

    [Header("Other References")]
    public MonoBehaviour mouseLookScript; // Assign your MouseLook script in inspector

    private bool isPaused = false;

    void Start()
    {
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

        Time.timeScale = 1f; // Ensure game starts unpaused
    }

    void Update()
    {
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
        Debug.Log("Pause triggered!"); // ✅ Helps confirm it's running

        if (mouseLookScript != null)
            mouseLookScript.enabled = false;

        if (StaminaBar != null)
            StaminaBar.SetActive(false);

        if (PauseScreen != null)
        {
            PauseScreen.SetActive(true);

            // If it has an Animator, make sure it updates even when Time.timeScale = 0
            Animator anim = PauseScreen.GetComponent<Animator>();
            if (anim != null)
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (mouseLookScript != null)
            mouseLookScript.enabled = true;

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

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenUI(GameObject uiToOpen)
    {
        if (uiToOpen != null)
        {
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

            uiToOpen.SetActive(true);
        }
    }
}
