using UnityEngine;

public class Key : MonoBehaviour
{

    public int KeyNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager player = other.GetComponent<InventoryManager>();
            if (player != null)
            {
                player.hasKey = true;
                player.CurrentKey = KeyNumber;
                Destroy(gameObject);
            }
        }
    }
}
