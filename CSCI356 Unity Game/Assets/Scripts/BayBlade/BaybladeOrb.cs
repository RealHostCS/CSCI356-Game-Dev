using UnityEngine;

public class BaybladeOrb : MonoBehaviour
{
    private GameObject player;
    
    public Sprite baybladeImage;

    [Header("UIChanging")]
    public UpdatePlayerUI updatePlayerUi;

   
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void Update()
    {
        
    }

    private void firstTimeCollectingOrb() 
    {
        var playerAttributes = player.GetComponent<InventoryManager>();
        updatePlayerUi.UpdateCollectionWords("Collected");
        if (baybladeImage != null)
                updatePlayerUi.SetOrbImageTransparency(1f);
                updatePlayerUi.UpdateOrbImage(baybladeImage);
        updatePlayerUi.UpdateNumberCount(1);
    }

    private void OnTriggerEnter(Collider other) 
    {
        var playerAttributes = player.GetComponent<InventoryManager>();
        if(!playerAttributes.collectedBlade)
        {
            firstTimeCollectingOrb();
            playerAttributes.collectedBlade = true;
        }
        if (other.CompareTag("Player"))
        {

            playerAttributes.CollectedBlades++;
            updatePlayerUi.UpdateNumberCount(playerAttributes.CollectedBlades);   
        }
        Destroy(gameObject);
    }
}
