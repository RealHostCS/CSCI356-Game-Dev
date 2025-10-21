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
            currentColor.a = Mathf.Clamp01(value); 
            orbImage.color = currentColor;
        }
    }

  
    public void UpdateNumberCount(int number)
    {
        if (numberCount != null)
            numberCount.text = number.ToString();
    }

  )
    public void UpdateCollectionWords(string words)
    {
        if (collectionWords != null)
            collectionWords.text = words;
    }
}
