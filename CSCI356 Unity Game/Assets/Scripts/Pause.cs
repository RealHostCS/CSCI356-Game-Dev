using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject Controls;
    public GameObject Audio;
    public GameObject Visual;
    public GameObject Random;
    public GameObject StaminaBar;

    public GameObject freezeCam;
    private bool isPaused = false;

    void Start()
    {
        // Hide the pause screen at the start
        PauseScreen.SetActive(false);
        Controls.SetActive(false);
        Audio.SetActive(false);
        Visual.SetActive(false);
        Random.SetActive(false);
        Time.timeScale = 1f; // Ensure time runs normally
    }

    void Update()
    {
        // Listen for ESC key press
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
        Debug.Log("Pause Screen Called");
        freezeCam.SetActive(false);
        StaminaBar.SetActive(false);
        PauseScreen.SetActive(true);
        Time.timeScale = 0f; // Stop time
        isPaused = true;
    }

    public void ResumeGame()
    {
        freezeCam.SetActive(true);
        PauseScreen.SetActive(false);
        StaminaBar.SetActive(true);
        Time.timeScale = 1f; // Resume time
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenControls()
    {
        Debug.Log("Button Pressed");
        Controls.SetActive(true);
        PauseScreen.SetActive(false);
    }
}
