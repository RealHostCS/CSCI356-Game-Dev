using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathScreenUI : MonoBehaviour
{
    private MonoBehaviour mouseLookScript; 
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene("SampleScene");

        // Find the player and reset it (after scene loads)
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        player.ResetPlayer();
    }

    public void QuitGame()
    {
        
        // Quit the game (works in build, not in editor)
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
