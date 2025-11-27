using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private TextMeshProUGUI count;

    private List<CardObj> playerCards = new List<CardObj>();

    private void Start()
    {
        MoveCards();

        ShuffleCards(playerCards);
    }

    private void MoveCards()
    {
        var deck = CardDeck.Instance.GetCards();
        if (deck == null)
            return;

        foreach (var c in deck)
        {
            playerCards.Add(c);
            c.transform.SetParent(cardParent, false);
        }
    }

    private void ShuffleCards(List<CardObj> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    public CardObj DrawCard()
    {
        if (playerCards.Count == 0)
        {
            RefillCards();

            if (playerCards.Count == 0)
            {
                UpdateCount();
                return null;
            }
        }

        var card = playerCards[0];
        playerCards.RemoveAt(0);

        HandManager.Instance.AddHand(card);

        UpdateCount();
        return card;
    }

    public void RefillCards()
    {
        var grave = FindFirstObjectByType<PlayerGrave>();

        Transform gravetranform = grave.transform.GetChild(0);

        var graveCards = gravetranform.GetComponentsInChildren<CardObj>(includeInactive: true);
        if (graveCards.Length == 0)
            return;

        foreach (var card in graveCards)
        {
            card.transform.SetParent(cardParent, false);

            card.gameObject.SetActive(false);

            playerCards.Add(card);
            grave.RemoveCard(card);
        }

        ShuffleCards(playerCards);
    }

    public void UpdateCount()
    {
        count.text = playerCards.Count.ToString();
    }

    public void AllReturnCardDeck()
    {
        foreach (var card in playerCards)
        {
            CardDeck.Instance.DeckReturnCard(card);
        }
        playerCards.Clear();
    }
}