using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    public AudioSource damageSound;
    public RawImage damageVignetteFromUI;

    public void StartDamage(int degree)
    {
        StartCoroutine(TakeDamageEffect(degree));
    }

    private IEnumerator TakeDamageEffect(int degree)
    {
        damageSound.Play();
        damageVignetteFromUI.enabled = true;
        gameObject.GetComponent<PlayerAttributes>().Damage(degree);
        yield return new WaitForSeconds(0.4f);
        damageVignetteFromUI.enabled = false;
        yield break;
    }
}
