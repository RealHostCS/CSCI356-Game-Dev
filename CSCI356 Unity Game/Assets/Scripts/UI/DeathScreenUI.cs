using UnityEngine;

public class DeathScreenUI : MonoBehaviour
{
    public void RestartGame()
    {
        // Ask GameManager to handle the restart
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager not found in scene!");
        }
    }

    public void QuitGame()
    {
        // Quit the game (works in build, not in editor)
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
