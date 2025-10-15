using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatePlayerUI : MonoBehaviour
{
    private GameObject player;

    [Header("UI Elements")]
    public TextMeshProUGUI  numberCount;
    public Image orbImage;
    public TextMeshProUGUI  collectionWords;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // 🟩 Updates the orb image shown on screen
    public void UpdateOrbImage(Sprite newSprite)
    {
        if (orbImage != null && newSprite != null)
            orbImage.sprite = newSprite;
    }

    public void SetOrbImageTransparency(float value)
    {
        if (orbImage != null)
        {
            Color currentColor = orbImage.color;
            currentColor.a = Mathf.Clamp01(value); // ensures the value stays between 0 and 1
            orbImage.color = currentColor;
        }
    }

    // 🟩 Updates the number text (e.g., number of orbs collected)
    public void UpdateNumberCount(int number)
    {
        if (numberCount != null)
            numberCount.text = number.ToString();
    }

    // 🟩 Updates the descriptive text (e.g., “Collected!”, “Orb Found!”, etc.)
    public void UpdateCollectionWords(string words)
    {
        if (collectionWords != null)
            collectionWords.text = words;
    }
}
