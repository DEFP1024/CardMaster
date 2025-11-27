using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerDeck playerDeck;
    [SerializeField] private Transform GraveParent;
    [SerializeField] private int drawCount;
    [SerializeField] private PlayerData player;
    [SerializeField] private int maxHandCard;


    private void Start()
    {
        
        SetUp();
    }

    public void SetUp()
    {
        HandManager.Instance.SetUpHand();
        player = GameManager.Instance.Player.PlayerData;
        StartDraw();
    }

    public void StartDraw()
    {
        for (int i = 0; i < player.Draw; i++)
        {
            CardDraw();
        }
    }

    public void CardDraw()
    {
        if (HandManager.Instance.HandCards.Count >= maxHandCard)
            return;

        CardObj card = playerDeck.DrawCard();

        if (card == null)
            return;

        card.gameObject.SetActive(true);

        HandManager.Instance.AddHand(card);
    }
}
