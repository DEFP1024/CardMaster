using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardStockManager : Singleton<CardStockManager>
{
    [SerializeField] private CardList cardList;

    [SerializeField] private int maxCard = 10;

    [SerializeField] private Transform poolParent;

    private List<CardtestObj> deck = new List<CardtestObj>();

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

        Debug.Log($"카드 풀 생성 완료. 총 인스턴스 수: {deck.Count}");
    }

    public CardtestObj TryTakeCard(CardtestObj target)
    {
        if (target == null)
            return null;

        foreach (var card in deck)
        {
            if (card.gameObject.activeSelf == false && card.Data == target.Data)
            {
                card.gameObject.SetActive (true);
                return card;
            }
        }

        return null;
    }

    public void ReturnCard(CardtestObj card)
    {
        if (card == null)
            return;

        card.transform.SetParent(poolParent, false);
        card.gameObject.SetActive(false);
        
        //if (currentCount >= maxCard) return;

        //deck.Add(card);
    }

    public int GetRemainCount(CardtestObj card)
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
}
