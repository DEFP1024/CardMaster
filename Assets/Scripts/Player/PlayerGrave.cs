using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGrave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;

    private List<CardObj> graveCards = new List<CardObj>();

    void Start()
    {
        GameManager.Instance.SetUpGrave();
        UpdateCount();
    }

   
    public void AddCard(CardObj card)
    {
        graveCards.Add(card);
        UpdateCount();
    }

    public void RemoveCard(CardObj card)
    {
        graveCards.Remove(card);
        UpdateCount();
    }

    public void UpdateCount()
    {
        count.text = graveCards.Count.ToString();
    }

    public void AllReturnCardDeck()
    {
        foreach (var card in graveCards)
        {
            CardDeck.Instance.DeckReturnCard(card);
        }
        graveCards.Clear();
    }
}
