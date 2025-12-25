using UnityEngine;

public class DoorWay : MonoBehaviour
{
    public StatTracker statTracker;   // Reference to your StatTracker script
    public int killsRequired = 3;     // Number of kills needed to open the door
    public GameObject door;           // Reference to the door object
    public GameObject wall;           // Reference to the wall object (optional)

    private bool doorOpened = false;

    void CheckStat()
    {
        // Make sure the StatTracker exists and the door isn’t already open
        if (statTracker != null && !doorOpened)
        {
            // Check if enemy kills meet or exceed the requirement
            if (statTracker.EnemyKillCount() >= killsRequired)
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        doorOpened = true;
        Debug.Log("Door opened! Colliders disabled.");

        // Disable the door's collider (if it has one)
        if (door != null)
        {
            Collider doorCollider = door.GetComponent<Collider>();
            if (doorCollider != null)
                doorCollider.enabled = false;
        }

        // Disable the wall's collider (optional)
        if (wall != null)
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            if (wallCollider != null)
                wallCollider.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Yup We know your trying to win ");
            CheckStat();
        }
    }
}
