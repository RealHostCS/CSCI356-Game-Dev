using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Image fillImage;
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public void SetHealth(float amount)
    {
        currentHealth = amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        fillImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
            Debug.Log("EnemyBeyblade is destroyed!");
    }
}