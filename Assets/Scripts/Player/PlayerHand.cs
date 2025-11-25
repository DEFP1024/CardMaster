using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerDeck playerDeck;
    [SerializeField] private Transform GraveParent;
    [SerializeField] private int drawCount;
    [SerializeField] private PlayerData player;
    [SerializeField] private int maxHandCard;

    private List<CardObj> playerCards = new List<CardObj>();

    private void Start()
    {
        StartDraw();
    }

    public void StartDraw()
    {
        for (int i = 0; i < drawCount; i++)
        {
            CardDraw();
        }
    }

    public void CardDraw()
    {
        if (playerCards.Count == maxHandCard)
            return;

            playerCards.Add(playerDeck.DrawCard());

    }

    public void Endturn()
    {
        foreach (var card in playerCards)
        {
            MoveGrave(card);
        }
    }

    private void MoveGrave(CardObj card)
    {
        card.transform.SetParent(GraveParent, false);
        playerCards.Remove(card);
    }

    public void UseCard(CardObj card)
    {
        MoveGrave(card);
    }
}
