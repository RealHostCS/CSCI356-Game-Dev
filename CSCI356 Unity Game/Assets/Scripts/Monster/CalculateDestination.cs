using UnityEngine;

public class CalculateDestination : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Player;

    private MonsterStats monsterstat;

    private MonsterState previousState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Always assign the reference in Start
        if (Monster != null)
        {
            monsterstate = Monster.GetComponent<MonsterStats>();
            if (monsterstat == null)
            {
                Debug.LogError("MonsterStats component not found on Monster GameObject!");
            }
        }
        else
        {
            Debug.LogError("Monster GameObject not assigned in Inspector!");
        }
    }

void Update()
{
    // 1. Handle one-time actions when entering a new state
    if (MonsterState != previousState)
    {
        OnStateEnter(MonsterState.);
        previousState = Monster;
    }

    // 2. Handle continuous actions that should run every frame
    OnStateUpdate(MonsterState);
}

void OnStateEnter(MonsterState newState)
{
    switch (newState)
    {
        case MonsterState.Angry:
            Debug.Log("Entered Angry state!");
            monster.speed = 20;
            break;

        case MonsterState.Scared:
            Debug.Log("Entered Scared state!");
            monster.speed = 20;
            break;

        case MonsterState.Attack:
            Debug.Log("Entered Attack state!");
            monster.speed = 14;
            break;

        case MonsterState.Hiding:
            Debug.Log("Entered Hiding state!");
            monster.speed = 14;
            setPositionHiding(); 
            break;

        case MonsterState.Stalk:
            Debug.Log("Entered stalking Entered!");
            monster.speed = 14;
            break;
    }
}

void OnStateUpdate(MonsterState currentState)
{
    switch (currentState)
    {
        case MonsterState.Angry:
            setPositionAngry();
            break;

        case MonsterState.Scared:
            setPositionScared();
            break;

        case MonsterState.Attack:
            setPositionAngry();
            break;

        case Default:
            break;
    }
}

    void setPositionAngry() {
        Monster.speed = 20;
    }
    void setPositionScared() {
        
    }
    void setPositionAttack() {
        player.position;
    }
    void setPositionHiding() {
        
    }
    void setPositionStalk() {
        
    }

}
