using UnityEngine;



[CreateAssetMenu(fileName = "Event", menuName = "Scriptable Objects/Event")]
public class EventData : ScriptableObject, IEvent
{
    [SerializeField] private EventType type;
    [SerializeField] private int hp = 0;
    [SerializeField] private int money = 0;
    [SerializeField] private CardList list;
    [TextArea]
    [SerializeField] string description;

   public EventType Type => type;

    public int Hp => hp;

    public int Money => money;

    public CardList List => list;

    public string Description => description;
}
