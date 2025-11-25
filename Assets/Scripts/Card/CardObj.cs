using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardObj : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;

    public CardData Data => cardData;

    private void Awake()
    {
        DataToUI();
    }

    public void DataToUI()
    {
        if (cardData == null)
            return;
        if (image != null)
            image.sprite = cardData.CardImage;
        if (typeText != null)
            typeText.text = cardData.Type.ToString();
        if (desText != null)
            desText.text = cardData.Description;
        if (costText != null)
            costText.text = cardData.Cost.ToString();
        if(nameText != null)
            nameText.text = cardData.Name;
    }
}
