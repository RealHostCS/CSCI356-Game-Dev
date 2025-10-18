using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 0.2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        Debug.Log("Hit Detected on " + other.name);

        // Optional: if your enemy still has a script for health
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Pot pot = other.GetComponent<Pot>();
        if (pot != null)
        {
            pot.DestroyPot();
        }
        
    }

}
