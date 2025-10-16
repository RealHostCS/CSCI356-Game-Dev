using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckBattles : MonoBehaviour
{
    public GameObject player;
    private GameObject mimic;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mimic = GameObject.FindGameObjectWithTag("Monster");
        
        if (player == null)
            Debug.LogWarning("Player not found! Check the tag.");
    }

    void Update()
    {
        
        if (player == null) return; // safety check

        var playerInventory = player.GetComponent<InventoryManager>();
        var MonterContact = mimic.GetComponent<PlayerContactLogic>();
        if (playerInventory == null) return; // safety check

        if (playerInventory.BaybladeBattlesWon >= 3)
        {
           SceneManager.LoadScene("Zelda");
        }
        else if (playerInventory.BaybladeBattlesLost >= 3)
        {
            MonterContact.playerColision = true;
            StartCoroutine(MonterContact.StopTimelineAfterSeconds(10f));
            MonterContact.HandlePlayerDeath(player);
        }
    }
}
