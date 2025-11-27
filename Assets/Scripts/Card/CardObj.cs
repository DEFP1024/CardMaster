using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Image highlightImage;

    [SerializeField ]private Canvas canvas;

    public CardData Data => cardData;

    public int Cost => cardData.Cost;

    public bool InHand { get; set; } = false;

    private Vector3 originalScale;
    private float plusScale = 1.2f;

    private int originalSortingOrder;

    private void Awake()
    {
        originalScale = transform.localScale;
        DataToUI();

        if (canvas == null)
            canvas = GetComponent<Canvas>();

        if (highlightImage != null)
            highlightImage.enabled = false;

        if (canvas != null)
        {
            originalSortingOrder = canvas.sortingOrder;
            canvas.overrideSorting = true;
        }
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

        canvas.sortingOrder = 500;

        transform.localScale = originalScale * plusScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InHand == false)
            return;

        ResetScale();
    }

    public void ResetScale()
    {
        canvas.sortingOrder = originalSortingOrder;
        transform.localScale = originalScale;
    }

    public void UpdateHighlight(int currentAP)
    {
        if (highlightImage == null)
            return;


            if (InHand == false)
        {
             highlightImage.enabled = false;
             return;
        }

        bool canPlay = Cost <= currentAP;
         highlightImage.enabled = canPlay;
    }

    public void RemoveHighlight()
    {
        if (highlightImage != null)
            highlightImage.enabled = false;
    }
    public void OnClickPlayCard()
    {
        var data = Data;

        if (data.CardTarget == CardTaget.Enemy)
        {
            CardTargetManager.Instance.StartEnemyTargeting(this);
        }

        else
        {
            GameManager.Instance.TryPlayCard(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        OnClickPlayCard();
    }
}
