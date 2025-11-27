using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] EventList eventList;
    private EventData currentEvent;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button skipButton;

    private CardObj rewardCard;

    private void Start()
    {
        currentEvent = GetRandomEvent();
        Setup(currentEvent);
    }

    private EventData GetRandomEvent()
    {
        if (eventList == null || eventList.events == null || eventList.events.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, eventList.events.Count);
        return eventList.events[index];
    }

    private void Setup(EventData eventData)
    {
        currentEvent = eventData;
        rewardCard = null;

        if (currentEvent != null && currentEvent.Type == EventType.AddCard)
        {
            RandomRewardCard();

            if (rewardCard == null)
            {
                var newEvent = GetRandomEvent();
                Setup(newEvent);
                return;
            }
        }
        RefreshUI();
    }

    private void RandomRewardCard()
    {
        if (currentEvent == null || currentEvent.List == null || currentEvent.List.cards == null)
            return;

        var Cards = new List<CardObj>();

        foreach (var card in currentEvent.List.cards)
        {
            if (card == null)
                continue;

            if (CardStockManager.Instance != null && CardStockManager.Instance.HasCard(card))
                Cards.Add(card);
        }

        if (Cards.Count == 0)
        {
            rewardCard = null;
            return;
        }

        int r = Random.Range(0, Cards.Count);
        rewardCard = Cards[r];
    }


    private void RefreshUI()
    {
        if (currentEvent == null)
            return;

        if (descriptionText != null)
            descriptionText.text = ReplaceText(currentEvent.Description);

        if (currentEvent.Type == EventType.Battle)
            skipButton.gameObject.SetActive(false);
        else if (currentEvent.Type == EventType.RemoveHP)
            skipButton.gameObject.SetActive(false);
    }

    private string ReplaceText(string text)
    {
        text = text.Replace("{Money}", currentEvent.Money.ToString());
        text = text.Replace("{Hp}", currentEvent.Hp.ToString());

        if (rewardCard != null)
            text = text.Replace("{Card}", rewardCard.Data.Name.ToString());

        return text;
    }

    public void OnClickAccept()
    {
        if (currentEvent == null)
            return;

        switch (currentEvent.Type)
        {
            case EventType.Battle:
                BattleEvent();
                break;
            case EventType.AddCard:
                AddCardEvent();
                break;
            case EventType.RemoveHP:
                RemoveHpEvent();
                break;
            case EventType.GetMoney:
                GetMoneyEvent();
                break;
        }
    }

    public void BattleEvent()
    {
        SceneChanger.Instance.OnStageScene(NodeType.Enemy);
    }

    public void RemoveHpEvent()
    {
        GameManager.Instance.ChangePlayerHP(currentEvent.Hp);
        BackStage();
    }

    public void AddCardEvent()
    {
        CardDeck.Instance.TakeCard(rewardCard);
        BackStage();
    }

    public void GetMoneyEvent()
    {
        GameManager.Instance.ChangePlayerMoney(currentEvent.Money);
        BackStage();
    }

    public void SkipEvent()
    {
        BackStage();
    }

    public void BackStage()
    {
        SceneChanger.Instance.OnStage();
    }
}
