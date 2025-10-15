using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Mimic))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Mimic Settings")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;

        [Tooltip("How quickly to smooth velocity changes")]
        public float velocityLerpCoef = 4f;

        [Header("Navigation Settings")]
        public Transform target; // Assign your player or another object here
        public float stoppingDistance = 1.5f;

        [Header("Stalk Settings")]
        public float minDistance = 50f;
        public float maxDistance = 60f;

        [Header("Hide Distance")]
        public float hideDistance = 100f;

        private NavMeshAgent agent;
        private Mimic myMimic;
        private Vector3 velocity = Vector3.zero;
        private MonsterStates MonsterStates;

        private bool hidingSpotFound = false;
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            myMimic = GetComponent<Mimic>();
            MonsterStates = GetComponent<MonsterStates>();

            // Sync agent speed with Mimic speed for consistency
            agent.speed = agent.speed == 0 ? 5f : agent.speed;
            agent.stoppingDistance = stoppingDistance;
        }

        private void Update()
        {
            if (MonsterStates != null)
            {
                // Pass the enum state to OnStateUpdate
                if (MonsterStates.currentMonsterState == MonsterStates.MonsterState.hiding && !hidingSpotFound)
                {
                    OnStateUpdate(MonsterStates.currentMonsterState);
                    hidingSpotFound = true;
                }
                if (!(MonsterStates.currentMonsterState == MonsterStates.MonsterState.hiding)) 
                {
                    OnStateUpdate(MonsterStates.currentMonsterState);
                    hidingSpotFound = false;
                }
            }


            // Calculate local velocity based on NavMeshAgent movement
            Vector3 desiredVelocity = agent.velocity;

            // Smooth out velocity transitions
            velocity = Vector3.Lerp(velocity, desiredVelocity, velocityLerpCoef * Time.deltaTime);

            // Update Mimic for proper leg animations
            myMimic.velocity = new Vector3(velocity.x, 0, velocity.z);

            // Keep the monster grounded at the proper height
            AdjustHeight();
        }

        private void AdjustHeight()
        {
            RaycastHit hit;
            Vector3 destHeight = transform.position;

            if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit))
            {
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            }

            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        void OnStateUpdate(MonsterStates.MonsterState currentMonsterState)
        {
            switch (currentMonsterState)
            {
                case MonsterStates.MonsterState.angry:
                    agent.SetDestination(target.position);
                    break;

                case MonsterStates.MonsterState.scared:
                    // Direction away from the target
                    Vector3 fleeDirection = (transform.position - target.position).normalized;

                    // Pick a point some distance away (e.g., 10 units away)
                    Vector3 fleePosition = transform.position + fleeDirection * 10f;

                    agent.SetDestination(fleePosition);
                    break;

                case MonsterStates.MonsterState.attack:
                    agent.SetDestination(target.position);
                    break;

                case MonsterStates.MonsterState.stalk:

                Vector3 toPlayer = target.position - transform.position;
                float distance = toPlayer.magnitude;

                if (distance < minDistance)
                {
                    // Too close -> move back out
                    Vector3 retreatDir = (transform.position - target.position).normalized;
                    Vector3 retreatPos = target.position + retreatDir * minDistance;
                    agent.SetDestination(retreatPos);
                }
                else if (distance > maxDistance)
                {
                    // Too far -> move closer
                    agent.SetDestination(target.position);
                }
                else
                {
                    // In stalk zone -> orbit around the player
                    float orbitSpeed = 40f; // degrees per second
                    float angle = orbitSpeed * Time.time; 

                    // Pick a point on a circle around the player
                    Vector3 orbitOffset = new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad),
                        0,
                        Mathf.Sin(angle * Mathf.Deg2Rad)
                    ) * distance;

                    Vector3 orbitPos = target.position + orbitOffset;
                    agent.SetDestination(orbitPos);
                }
                break;

                case MonsterStates.MonsterState.hiding:
                        Vector3 hideDir = (transform.position - target.position).normalized;
                        Vector3 hidePos = target.position + hideDir * hideDistance;

                        agent.SetDestination(hidePos);
                    break;
            }
        }
        
    }
}