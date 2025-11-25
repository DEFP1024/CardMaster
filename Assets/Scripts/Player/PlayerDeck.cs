using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private Transform handParent;
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
       for (int i = list.Count -1; i >= 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    public CardObj DrawCard()
    {
        if (playerCards.Count == 0)
            return null;

        var card = playerCards[0];
        playerCards.RemoveAt(0);

        card.transform.SetParent(handParent, false);
        UpdateCount();
        return card;
    }

    public void UpdateCount()
    {
        count.text = playerCards.Count.ToString();
    }
}
