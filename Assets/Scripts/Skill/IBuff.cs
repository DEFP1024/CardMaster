using UnityEngine;

public enum BuffType
{
    None,
    Heal
}

public interface IBuff
{
    public BuffType _BuffType { get; }
}
