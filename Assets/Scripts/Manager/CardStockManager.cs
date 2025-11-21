using System.Collections.Generic;
using UnityEngine;

public class CardStockManager : Singleton<CardStockManager>
{
    [SerializeField] private CardList cardList;

    [SerializeField] private int maxCard = 10;

    private List<CardData> deck = new List<CardData>();

    protected override void Awake()
    {
        base.Awake();
        BuildDeck();
    }

    private void BuildDeck()
    {
        deck.Clear();

        if (cardList == null || cardList.cards == null)
            return;

        foreach (var card in cardList.cards)
        {
            if (card == null) continue;

            for (int i = 0; i < maxCard; i++)
            {
                deck.Add(card);
            }
        }
    }

    public bool TryTakeCard(CardData target)
    {
        if (target == null)
            return false;

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i] == target)
            {
                deck.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void ReturnCard(CardData card)
    {
        if (card == null)
            return;

        int currentCount = GetRemainCount(card);
        if (currentCount >= 0) return;

        deck.Add(card);
    }

    public int GetRemainCount(CardData card)
    {
        int cnt = 0;
        foreach (var d in deck)
        {
            if (d == card) cnt++;
        }    

        return cnt;
    }
}
