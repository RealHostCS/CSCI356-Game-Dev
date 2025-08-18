using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        // Subscribe to sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe immediately
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Use coroutine to wait one frame before resetting
        StartCoroutine(ResetPlayerNextFrame());
    }

    private IEnumerator ResetPlayerNextFrame()
    {
        yield return null; // Wait one frame

        // Reset PlayerMovement
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ResetPlayer();
            playerMovement.enabled = true;
        }

        // Reset PlayerAttributes
        PlayerAttributes playerAttributes = FindAnyObjectByType<PlayerAttributes>();
        if (playerAttributes != null)
        {
            playerAttributes.health = 100;
            playerAttributes.isDead = false;
        }

        // Reset Dying UI
        Dying dyingUI = FindAnyObjectByType<Dying>();
        if (dyingUI != null)
        {
            if (dyingUI.deathScreenUI != null)
                dyingUI.deathScreenUI.SetActive(false);
            if (dyingUI.sprintBar != null)
                dyingUI.sprintBar.SetActive(true);
        }

        // Resume game time and cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
