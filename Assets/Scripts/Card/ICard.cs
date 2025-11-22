using System.Collections;
using UnityEngine;
public enum CardType
{
    Attack,
    Skill,
    Armor
}

public interface ICard
{
    public int ID { get; }
    public string Name { get; }
    public int Cost { get; }
    public int Power { get; }
    public string Description {  get; }
    CardType Type { get; }
}
