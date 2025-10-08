using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseNavMesh : MonoBehaviour
{
    public Transform player;       // assign the player in inspector
    public float updateRate = 0.2f; // how often to update pathfinding (seconds)

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 4f;
    }

    void Update()
    {
        if (player == null || agent == null) return;

            timer += Time.deltaTime;
        if (timer >= updateRate)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance > agent.stoppingDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.ResetPath(); // stop moving
                    Attack();          // call your attack function
                }

                timer = 0f;
            }
    }   

    void Attack()
    {
        // Play attack animation or deal damage
        Debug.Log("Attack!");
    }

}
