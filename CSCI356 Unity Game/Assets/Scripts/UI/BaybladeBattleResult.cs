using UnityEngine;
using TMPro;
using System.Collections;

public class BaybladeBattleResult : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI resultText; // assign in inspector

    /// <summary>
    /// Call this method when the battle ends.
    /// </summary>
    /// <param name="didWin">True if player won, false if lost.</param>
    public void ShowResult(bool didWin)
    {
        if (resultText == null)
        {
            Debug.LogWarning("ResultText not assigned!");
            return;
        }

        resultText.text = didWin ? "You won the Bayblade battle!" : "You lost the Bayblade battle!";
        StartCoroutine(ClearTextAfterSeconds(1.5f));
    }

    private IEnumerator ClearTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        resultText.text = "";
    }
}
