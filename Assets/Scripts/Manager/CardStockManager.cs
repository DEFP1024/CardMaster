using System.Collections.Generic;
using UnityEngine;

public class CardStockManager : Singleton<CardStockManager>
{
    [SerializeField] private CardList cardList;

    [SerializeField] private int maxCard = 10;

    [SerializeField] private Transform poolParent;

    public List<CardObj> deck = new List<CardObj>();

    private GameObject cardPrefab;

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
                var inst = Instantiate(card, poolParent);
                inst.gameObject.SetActive(false);
                deck.Add(inst);
            }
        }
    }

    public CardObj TryTakeCard(CardObj target)
    {
        if (target == null)
            return null;

        foreach (var card in deck)
        {
            if (card.Data == target.Data && card.transform.parent == poolParent)
            {
                //card.gameObject.SetActive(true);
                return card;
            }
        }

        return null;
    }

    public void ReturnCard(CardObj card)
    {
        if (card == null)
            return;

        card.transform.SetParent(poolParent, false);
        card.gameObject.SetActive(false);
    }

    public int GetRemainCount(CardObj card)
    {

        if (card == null)
        {
            return 0;
        }

        int cnt = 0;

        foreach (var d in deck)
        {
            if (d.gameObject.activeSelf == false && d.Data == card.Data)
                cnt++;
        }    

        return cnt;
    }

    public bool HasCard (CardObj card)
    {
        if (card == null)
            return false;

        return GetRemainCount(card) > 0;
    }
}
