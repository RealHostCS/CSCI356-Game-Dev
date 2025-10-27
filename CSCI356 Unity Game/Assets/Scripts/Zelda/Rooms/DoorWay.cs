using UnityEngine;

public class DoorWay : MonoBehaviour
{
    public StatTracker statTracker;  // Reference to your StatTracker script
    public int killsRequired = 3;    // Number of kills needed to open the door
    public GameObject door;          // Reference to the door object

    private bool doorOpened = false;

    void Update()
    {
    }

    void CheckStat()
    {
        // Make sure the StatTracker exists and the door isn’t already open
        if (statTracker != null && !doorOpened)
        {
            // Check if enemy kills meet or exceed the requirement
            if (statTracker.enemyKillCount >= killsRequired)
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        doorOpened = true;
        Debug.Log("Door opened!");

        // Example: disable the door object
        if (door != null)
        {
            door.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckStat();
        }
    }
}
