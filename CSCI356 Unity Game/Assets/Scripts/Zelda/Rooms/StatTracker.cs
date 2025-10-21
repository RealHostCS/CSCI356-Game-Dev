using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public int enemysKilled = 0;
    public int currentRoomNumber;
    public int newRoomNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnemyKilled()
    {
        enemysKilled += 1;
    }

    public void ChangeRoom(int room)
    {
        newRoomNumber = room;
        currentRoomNumber = newRoomNumber;
    }
}
