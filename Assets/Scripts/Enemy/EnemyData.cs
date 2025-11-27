using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject, IEnemy,IBuff, IDebuff
{
    [SerializeField] private int hp;
    [SerializeField] private int dmage;
    [SerializeField] private int skillPower;
    [SerializeField] private int armorPower;
    [SerializeField] private int armor;
    [SerializeField] private BuffType bufftype;
    [SerializeField] private DebuffType debuffType;
    [SerializeField] private ActionType[] action;
    [SerializeField] private MonsterType monsterType;

    public BuffType _BuffType => bufftype;

    public DebuffType _DeBuffType => debuffType;

    public int Dmage => dmage;

    public int Hp => hp;

    public int SkillPower => skillPower;

    public int ArmorPower => armorPower;

    public int Armor => armor;

    public ActionType[] _ActionType => action;

    public MonsterType _MonsterType => monsterType;
}
