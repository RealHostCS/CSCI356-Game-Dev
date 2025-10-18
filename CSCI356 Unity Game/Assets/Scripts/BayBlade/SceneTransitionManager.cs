using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public PlayerProximityChecker proximityChecker;

    public string miniGameSceneName = "BaybladeBattle";
    private Scene mainScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartMinigame()
    {
        StartCoroutine(LoadMinigame());
    }

    private IEnumerator LoadMinigame()
    {
        mainScene = SceneManager.GetActiveScene();

        // Pause main world
       

        // Hide everything in the main scene
        foreach (GameObject rootObj in mainScene.GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

        // Load minigame scene
        yield return SceneManager.LoadSceneAsync(miniGameSceneName, LoadSceneMode.Additive);

        Scene miniGameScene = SceneManager.GetSceneByName(miniGameSceneName);
        SceneManager.SetActiveScene(miniGameScene);
    }


    public void ReturnFromMinigame(bool battleWon)
    {
        StartCoroutine(UnloadMinigame(battleWon));
    }

    private IEnumerator UnloadMinigame(bool battleWon)
    {
        yield return SceneManager.UnloadSceneAsync(miniGameSceneName);

        // Reactivate everything in the main scene
        foreach (GameObject rootObj in mainScene.GetRootGameObjects())
        {
            rootObj.SetActive(true);
        }

        var playerInventory = Object.FindFirstObjectByType<InventoryManager>();
        var battleUI = FindFirstObjectByType<BaybladeBattleResult>();
        if (battleWon)
        {
            battleUI.ShowResult(battleWon);
            playerInventory.BaybladeBattlesWon += 1;
        }
        else 
        {
            battleUI.ShowResult(battleWon);
            playerInventory.BaybladeBattlesLost += 1;
        }
        // Reset every object that has a ResettableObject script
        foreach (var resettable in Object.FindObjectsByType<ResettableObject>(FindObjectsSortMode.None))
        {

            resettable.ResetToStart(proximityChecker.IsPlayerInRange());
        }
        // Resume g
        SceneManager.SetActiveScene(mainScene);
    }

}
