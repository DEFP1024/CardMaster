using UnityEngine;

public class CardObj : MonoBehaviour
{
    [SerializeField] private CardData cardData;

    public CardData Data => cardData;

    public void OnTakeCard()
    {
        CardDeck.Instance.TakeCard(this);
    }

    public void OnReturnCard()
    {
        CardDeck.Instance.ReturnCard(this);
    }
}
