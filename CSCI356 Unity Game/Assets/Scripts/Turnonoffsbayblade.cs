using UnityEngine;

public class TurnOnOffsBayblade : MonoBehaviour
{
    public GameObject player;

    void Awake()
    {
        InventoryManager inventory = player.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.isBayblade = true;
        }
        else
        {
            Debug.LogWarning("InventoryManager component not found on player!");
        }
    }
}
