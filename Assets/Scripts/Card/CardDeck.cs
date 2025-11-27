using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardDeck : Singleton<CardDeck>
{
    [SerializeField] private CardList cardList;

    private List<CardObj> deckCards = new List<CardObj>();

    public bool StartComplete = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartGame()
    {
        foreach (var card in cardList.cards)
        {
            if (card == null) continue;

            TakeCard(card);
        }
    }

    // 덱에 카드 추가
    public void TakeCard(CardObj card)
    {
        var instance = CardStockManager.Instance.TryTakeCard(card);

        if (instance != null)
        {
            instance.transform.SetParent(transform, false);
            deckCards.Add(instance);
        }
    }
    // 카드 되돌리기
    public void ReturnCard(CardObj selectedCard)
    {
        if (selectedCard == null)
            return;

        if (deckCards.Contains(selectedCard) == false)
            return;

        deckCards.Remove(selectedCard);
        CardStockManager.Instance.ReturnCard(selectedCard);
    }

    public void DeckReturnCard(CardObj card)
    {
        card.InHand = false;
        card.RemoveHighlight();

        card.transform.SetParent(transform, false);
        card.gameObject.SetActive(false);

        if (deckCards.Contains(card) == false)
            deckCards.Add(card);
    }

    public List<CardObj> GetCards()
    {
        return deckCards;
    }

    public void ClearCards()
    {
        deckCards.Clear();
    }

}
