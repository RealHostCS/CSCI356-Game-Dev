using UnityEngine;

public class CalculateDestination : MonoBehaviour
{
    public GameObject monster;
    public GameObject player;
    private UnityEngine.AI.NavMeshAgent agent;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Always assign the reference in Start
        /**
        if (Monster != null)
        {
            monsterstate = Monster.GetComponent<MonsterStates>();
            if (monsterstate == null)
            {
                Debug.LogError("MonsterStates component not found on Monster GameObject!");
            }
        }
        else
        {
            Debug.LogError("Monster GameObject not assigned in Inspector!");
        }
        */
    }

void Update()
{
}
/*
    void setPositionAngry() {
        agent.target = player;
    }
    void setPositionScared() {
        agent.target = player;
    }
    void setPositionAttack() {
        agent.target = player;
    }
    void setPositionHiding() {
        agent.target = player;
    }
    void setPositionStalk() {
        agent.target = player;
    }
    */
}
