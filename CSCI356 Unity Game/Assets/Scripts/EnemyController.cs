using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;      // Assign your player object in the Inspector
    public NavMeshAgent agent;

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}