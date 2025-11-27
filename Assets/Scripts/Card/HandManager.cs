using System.Collections.Generic;
using UnityEngine;

public class HandManager : Singleton<HandManager>
{
    [SerializeField] private Transform handParent;


    private List<CardObj> handCards =  new List<CardObj>();

    public List<CardObj> HandCards => handCards;

    protected override void Awake()
    {
        base.Awake();
        handCards.Clear();
    }

    public void SetUpHand()
    {
        handParent = FindAnyObjectByType<PlayerHand>().transform.GetChild(0);
    }

    public void RefreshHandCards()
    {
        var playerHand = FindFirstObjectByType<PlayerHand>();

        handParent = playerHand.transform.GetChild(0);

        handCards.Clear();

        var cards = handParent.GetComponentsInChildren<CardObj>(includeInactive: false);

        foreach (var card in cards)
        {
            card.InHand = true;
            handCards.Add(card);
        }

        UpdateHighlights(GameManager.Instance.PlayerAP);
    }

    public void AddHand(CardObj card)
    {
        card.transform.SetParent(handParent, false);
        card.InHand = true;

        if (handCards.Contains(card) == false)
            handCards.Add(card);

        if (GameManager.Instance != null)
            card.UpdateHighlight(GameManager.Instance.PlayerAP);
    }

    public void RemoveHand(CardObj card)
    {
        card.InHand = false;

        if (handCards.Contains(card))
            handCards.Remove(card);

        card.ResetScale();
        card.RemoveHighlight();
    }

    public void UpdateHighlights(int currentAP)
    {
        foreach (var card in handCards)
        {
            if (card != null)
                card.UpdateHighlight(currentAP);
        }
    }

    public void AllGrave()
    {
        var cards = new List<CardObj>(handCards);

        foreach (var card in cards)
        {
            GameManager.Instance.MoveGrave(card);
            RemoveHand(card);
        }
    }

    public void ReturnDeck()
    {
        handCards.Clear();
    }
}
