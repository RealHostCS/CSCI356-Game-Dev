// using UnityEngine;

// [RequireComponent(typeof(Collider))]
// public class Ice : MonoBehaviour
// {
//     [Header("Sliding Settings")]
//     public float slideFactor = 1.5f; // 1 = normal speed, >1 = faster/slippery

//     private void OnTriggerEnter(Collider other)
//     {
//         PlayerMovement player = other.GetComponent<PlayerMovement>();
//         if (player != null)
//         {
//             player.onIce = true;
//             player.iceSlideFactor = slideFactor;
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         PlayerMovement player = other.GetComponent<PlayerMovement>();
//         if (player != null)
//         {
//             player.onIce = false;
//             player.iceSlideFactor = 1f; // reset to normal
//         }
//     }
// }
