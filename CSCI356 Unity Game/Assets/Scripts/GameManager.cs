using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        // Singleton pattern to make sure there's only one GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Restarts the current scene and resets the player after reload.
    /// </summary>
    public void RestartGame()
    {
        // Reload the active scene
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe immediately so this only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Find the player and reset them
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.ResetPlayer();
        }
    }
}
