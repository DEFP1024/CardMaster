using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
public class PlayerData : ScriptableObject,IPlayer
{
    [SerializeField] int hp;
    [SerializeField] int ap;
    [SerializeField] int money;
    [SerializeField] int armor;
    [SerializeField] int draw;

    public int Hp => hp;

    public int AP => ap;

    public int Money => money;

    public int Armor => armor;

    public int Draw => draw;
}
