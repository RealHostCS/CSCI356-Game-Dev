using UnityEngine;
using UnityEngine.SceneManagement;

public class Dying : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathScreenUI;
    public GameObject sprintBar;

    private PlayerAttributes attributes;
    private GameManager gameManager;
    private PlayerContactLogic monster;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Find GameManager and player references
        gameManager = FindAnyObjectByType<GameManager>();
        attributes = FindAnyObjectByType<PlayerAttributes>();
        monster = FindAnyObjectByType<PlayerContactLogic>();

        deathScreenUI.SetActive(false);
        sprintBar.SetActive(true);
    }

    private void Update()
    {
        if (monster != null && monster.playerColision && attributes != null && attributes.health <= 0)
        {
            ShowDeathScreen();
        }
    }

    public void ShowDeathScreen()
    {
    
        deathScreenUI.SetActive(true);

        sprintBar.SetActive(false);

        // Stop the game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player movement
        if (attributes != null)
        {
            PlayerMovement movement = attributes.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = false;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart Log");
        if (gameManager != null)
            gameManager.RestartGame();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Called automatically after scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-find player & monster
        attributes = FindAnyObjectByType<PlayerAttributes>();
        monster = FindAnyObjectByType<PlayerContactLogic>();

        // Reset player
        if (attributes != null)
        {
            attributes.health = 100;
            attributes.isDead = false;

            PlayerMovement movement = attributes.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = true;
        }

        // Reset UI
        deathScreenUI.SetActive(false);
        sprintBar.SetActive(true);

        // Resume time
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset monster collision flag
        monster.playerColision = false;
    }
}
