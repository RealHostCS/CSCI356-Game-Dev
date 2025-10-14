using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage; // Assign HealthBarFill
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public void SetHealth(float amount)
    {
        currentHealth = amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        fillImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
            Debug.Log("Beyblade is destroyed!");
    }
}
