using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Flash Effect")]
    public float flashDuration = 0.2f;

    public float maxFlash = 1f;

    private Material mat;

    private bool isFlashing = false;


    private SpriteRenderer spriteRenderer;
    private MeshRenderer meshRenderer;
    private Animator animator;

    private Color originalColor;


    void Awake()
    {
        currentHealth = maxHealth;

        mat = GetComponentInChildren<SpriteRenderer>().material;

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (!isFlashing)
            StartCoroutine(Flash());

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.15f, 0.1f);
            
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private IEnumerator Flash()
    {
        isFlashing = true;

        mat.SetFloat("_FlashAmount", maxFlash);
        yield return new WaitForSeconds(flashDuration / 2f);

        mat.SetFloat("_FlashAmount", 0f);
        yield return new WaitForSeconds(flashDuration / 2f);

        isFlashing = false;
    }
}
