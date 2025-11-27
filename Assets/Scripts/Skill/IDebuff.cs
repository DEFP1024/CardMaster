using UnityEngine;

public enum DebuffType
{
    None,
    Weakness
}

public interface IDebuff
{
    public DebuffType _DeBuffType { get;}
}
