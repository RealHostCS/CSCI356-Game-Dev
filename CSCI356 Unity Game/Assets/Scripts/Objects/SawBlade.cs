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
        if (!other.CompareTag("Player"))
            return;

        PlayerAttributes player = other.GetComponent<PlayerAttributes>();
        if (player == null || player.isDead)
            return;

        // Match monster death logic: set health to 0 and call Die()
        player.health = 0;
        player.Die();

        // Mark monster collision so Dying.Update detects the death
        PlayerContactLogic monster = FindAnyObjectByType<PlayerContactLogic>();
        if (monster != null)
            monster.playerColision = true;

        // Immediately show the death UI (stops time, unlocks cursor, disables movement)
        Dying dyingUI = FindAnyObjectByType<Dying>();
        if (dyingUI != null)
            dyingUI.ShowDeathScreen();
    }
}
