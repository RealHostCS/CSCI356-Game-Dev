using UnityEngine;

public class SawBlade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f; // degrees per second

    private void Update()
    {
        // Spin the saw blade around the Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player
        if (other.CompareTag("Player"))
        {
            PlayerAttributes player = other.GetComponent<PlayerAttributes>();

            if (player != null && !player.isDead)
            {
                // Match monster death logic: set health to 0 and call Die()
                player.health = 0;
                player.Die();

                // Optionally mark monster collision so other systems that check it see the death
                PlayerContactLogic monster = FindAnyObjectByType<PlayerContactLogic>();
                if (monster != null)
                    monster.playerColision = true;

                // Show the death UI (stops time, unlocks cursor, disables movement)
                Dying dyingUI = FindAnyObjectByType<Dying>();
                if (dyingUI != null)
                    dyingUI.ShowDeathScreen();

                Debug.Log("Player killed by saw blade (health = 0), Die() called and death UI shown.");
            }
        }
    }
}
