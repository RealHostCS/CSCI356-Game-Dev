using System;
using UnityEngine;

public class MonsterStates : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    public enum MonsterState
    {
        angry,
        scared,
        attack,
        hiding,
        stalk,
        Null
    }

    [Header("Angry States")]
        
    [Header("Scared States")]
    [Header("Attack States")]
    [Header("Hiding States")]
    private float chance60Percent = 0.6f;
    private float chance10Percent = 0.9f;

    public MonsterState currentMonsterState = MonsterState.stalk;
    private MonsterState previousMonsterState = MonsterState.Null;

    private float stateTimer = 0f;          // how long we’ve been in current state
    private bool firstFrameInState = false; // flag for first frame of state

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        // Detect state change
        if (currentMonsterState != previousMonsterState)
        {
            stateTimer = 0f;
            firstFrameInState = true;
            previousMonsterState = currentMonsterState;
        }

        // Update timer
        stateTimer += Time.deltaTime;

        // Run logic for current state
        switch (currentMonsterState)
        {
            case MonsterState.angry:
                handleMonsterStateAngry();
                break;
            case MonsterState.scared:
                handleMonsterStateScared();
                break;
            case MonsterState.attack:
                handleMonsterStateAttack();
                break;
            case MonsterState.hiding:
                handleMonsterStateHiding();
                break;
            case MonsterState.stalk:
                handleMonsterStateStalking();
                break;
        }

        // Reset first-frame flag after Update runs once
        firstFrameInState = false;
    }

    // ------------------------
    // STATE HANDLERS
    // ------------------------

    void handleMonsterStateAngry()
    {
        if (firstFrameInState)
        {
            Debug.Log("Entered Angry state!");
            agent.speed = 20;
        }

        if (stateTimer >= 10f)
        {
            float roll = UnityEngine.Random.value;
            if (roll < chance60Percent)
            {
                ChangeState(MonsterState.stalk);
            }
            else if (roll > chance10Percent)
            {
                ChangeState(MonsterState.hiding);
            }
            else
            {
                ChangeState(MonsterState.attack);
            }
        }
    }

    void handleMonsterStateScared()
    {
        if (firstFrameInState)
        {
            Debug.Log("Entered Scared state!");
            agent.speed = 20;
        }

        if (stateTimer >= 10f)
        {
            if (UnityEngine.Random.value < chance60Percent)
            {
                ChangeState(MonsterState.stalk);
            }
            else
            {
                ChangeState(MonsterState.hiding);
            }
        }
    }

    void handleMonsterStateAttack()
    {
        if (firstFrameInState)
        {
            Debug.Log("Entered Attack state!");
            agent.speed = 14;
        }

        if (stateTimer >= 30f)
        {
            float roll = UnityEngine.Random.value;
            if (roll < chance60Percent)
            {
                ChangeState(MonsterState.stalk);
            }
            else if (roll > chance10Percent)
            {
                ChangeState(MonsterState.angry);
            }
            else
            {
                ChangeState(MonsterState.hiding);
            }
        }
    }

    void handleMonsterStateHiding()
    {
        if (firstFrameInState)
        {
            Debug.Log("Entered Hiding state!");
            agent.speed = 14;
            
        }

        if (stateTimer >= 20f)
        {
            ChangeState(MonsterState.stalk);
        }
    }

    void handleMonsterStateStalking()
    {
        if (firstFrameInState)
        {
            Debug.Log("Entered Stalk state!");
            agent.speed = 14;
        }

        if (stateTimer >= 20f)
        {
            if (UnityEngine.Random.value < chance60Percent)
            {
                ChangeState(MonsterState.attack);
            }
            else
            {
                ChangeState(MonsterState.hiding);
            }
        }
    }

    // ------------------------
    // HELPERS
    // ------------------------

    void ChangeState(MonsterState newMonsterState)
    {
        currentMonsterState = newMonsterState;
    }

    public MonsterState GetCurrentState()
    {
        return currentMonsterState;
    }
}
