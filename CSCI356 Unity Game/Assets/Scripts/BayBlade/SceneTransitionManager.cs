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

       
       

   
        foreach (GameObject rootObj in mainScene.GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

      
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
      
        foreach (var resettable in Object.FindObjectsByType<ResettableObject>(FindObjectsSortMode.None))
        {

            resettable.ResetToStart(proximityChecker.IsPlayerInRange());
        }
     
        SceneManager.SetActiveScene(mainScene);
    }

}
