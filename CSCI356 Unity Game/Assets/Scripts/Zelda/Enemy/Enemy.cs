using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Flash Effect")]
    public float flashDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private MeshRenderer meshRenderer;
    private Animator animator;

    private Color originalColor;
    private bool isFlashing = false;

    void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        animator = GetComponent<Animator>();

        // Save original color
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        else if (meshRenderer != null)
            originalColor = meshRenderer.material.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (!isFlashing)
            StartCoroutine(Flash());

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private IEnumerator Flash()
    {
        isFlashing = true;

        // Disable animator temporarily if it exists
        if (animator != null)
            animator.enabled = false;

        // Flash white
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
        else if (meshRenderer != null)
            meshRenderer.material.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        // Restore original color
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
        else if (meshRenderer != null)
            meshRenderer.material.color = originalColor;

        // Re-enable animator
        if (animator != null)
            animator.enabled = true;

        isFlashing = false;
    }
}
