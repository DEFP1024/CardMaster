using UnityEngine;

public enum ActionType
{
    Attack,
    Skill,
    Armor
}

public enum MonsterType
{
    Enemy,
    Boss
}

public interface IEnemy
{
    int Dmage { get; }
    int Hp { get; }
    int SkillPower { get; }
    int ArmorPower { get; }
    int Armor { get; }
    ActionType[] _ActionType { get; }
    MonsterType _MonsterType { get; }

}
