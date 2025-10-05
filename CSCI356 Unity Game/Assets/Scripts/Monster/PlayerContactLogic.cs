using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class PlayerContactLogic : MonoBehaviour
{
    private GameObject player;
    public PlayableDirector director;
    public bool playerColision = false;

    [Header("UIChanging")]
    public UpdatePlayerUI updatePlayerUi;
    
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
    } 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerDeath(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerColision = true;

            var baybladeComponent = other.GetComponent<PlayerAttributes>();
            bool isBayblade = false;
            int CollectedBlades = 0;
            if (baybladeComponent != null)
                    isBayblade = baybladeComponent.isBayblade;
                    CollectedBlades = baybladeComponent.CollectedBlades;

                if (isBayblade && CollectedBlades > 0)
                {
                    if (director != null)
                        CollectedBlades--;
                        PlayerAttributes playerAttributes = player.GetComponent<PlayerAttributes>();
                        updatePlayerUi.UpdateNumberCount(playerAttributes.CollectedBlades); 
                        director.Play();
                }
                else
                {
                    StartCoroutine(StopTimelineAfterSeconds(10f));
                    HandlePlayerDeath(other.gameObject);
                }
        }
    }

    private void HandlePlayerDeath(GameObject playerObj)
    {
        // Get the PlayerAttributes script instead
        PlayerAttributes playerAttributes = playerObj.GetComponent<PlayerAttributes>();
        if (playerAttributes != null)
        {

            playerAttributes.health = 0; // Set health to zero
            playerAttributes.Die();      // Trigger death logic
        }
        else
        {
            Debug.LogWarning("PlayerContactLogic: PlayerAttributes script not found on player object!");
        }
    }

    private IEnumerator StopTimelineAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        director.Stop();
    }
}
