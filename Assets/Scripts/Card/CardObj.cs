using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Image highlightImage;

    public CardData Data => cardData;

    public int Cost => cardData.Cost;

    public bool InHand { get; set; } = false;

    private Vector3 originalScale;
    private float plusScale = 1.2f;

    private void Awake()
    {
        originalScale = transform.localScale;
        DataToUI();
        highlightImage.enabled = false;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InHand == false)
            return;

        transform.localScale = originalScale * plusScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InHand)
            return;

        transform.localScale = originalScale;
    }

    public void UpdateHighlight(int currentAP)
    {
        if (InHand == false)
        {
            if (highlightImage != null)
                highlightImage.enabled = false;

            else
            {
                var img = highlightImage.GetComponent<Image>();
                if (img != null)
                    img.color = Color.springGreen;
            }

            return;
        }

        bool canPlay = Cost <= currentAP;

        if (highlightImage != null)
        {
            highlightImage.enabled = true;
        }
    }
}
