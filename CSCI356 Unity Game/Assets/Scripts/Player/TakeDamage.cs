using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    public AudioSource damageSound;
    public RawImage damageVignette;

    public void StartDamage(int degree)
    {
        StartCoroutine(TakeDamageEffect(degree));
    }

    private IEnumerator TakeDamageEffect(int degree)
    {
        damageSound.Play();
        damageVignette.enabled = true;
        gameObject.GetComponent<PlayerAttributes>().Damage(degree);
        yield return new WaitForSeconds(0.4f);
        damageVignette.enabled = false;
        yield break;
    }
}
