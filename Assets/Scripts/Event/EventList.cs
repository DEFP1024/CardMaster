using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventList", menuName = "Scriptable Objects/EventList")]
public class EventList : ScriptableObject
{
    public List<EventData> events;
}
