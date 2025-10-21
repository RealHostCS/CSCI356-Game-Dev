using UnityEngine;

public class AcidScript : MonoBehaviour
{
    public int count = 0;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            collider.GetComponent<PlayerMovement>().StartSlow(0.5f);
            collider.GetComponent<TakeDamage>().StartDamage(5);
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        count++;
        if (count > 50)
        {
            collider.GetComponent<TakeDamage>().StartDamage(5);
            count = 0;
        }
        
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.name == "Player")
        {
            collider.GetComponent<PlayerMovement>().ResetSpeed();
            count = 0;
        }
    }
}
