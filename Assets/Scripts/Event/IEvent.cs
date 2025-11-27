using UnityEngine;

public enum EventType
{
    Battle,
    AddCard,
    RemoveHP,
    GetMoney
}

public interface IEvent
{
    public EventType Type { get; }
    public int Hp { get; }
    public int Money { get; }
    public string Description {  get; }
    public CardList List { get; }
}
