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

        private NavMeshAgent agent;
        private Mimic myMimic;
        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            myMimic = GetComponent<Mimic>();

            // Sync agent speed with Mimic speed for consistency
            agent.speed = agent.speed == 0 ? 5f : agent.speed;
            agent.stoppingDistance = stoppingDistance;
        }

        private void Update()
        {
            if (target != null)
            {
                // Set the AI's destination to the target
                agent.SetDestination(target.position);
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
    }
}