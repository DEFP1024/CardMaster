using System;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public int CurrentHp { get; private set; }
    public int CurrentArmor { get; private set; }
    public int CurrentDamage => enemyData.Dmage;
    public int CurrentSkillPower => enemyData.SkillPower;

    public EnemyData Data => enemyData;
    public bool IsDead => CurrentHp <= 0;

    private int actionIndex = 0;

    public event Action<int> OnHpChanged;
    public event Action<int> OnArmorChanged;
    private void Awake()
    {
        ResetStatus();
        var ui = FindAnyObjectByType<EnemyUI>();
        ui.SetUp(this);
        OnHpChanged?.Invoke(CurrentHp);
    }
    public void ResetStatus()
    {
        if (enemyData == null)
            return;

        CurrentHp = enemyData.Hp;
        CurrentArmor = enemyData.Armor;
    }

    public void TakeDamage(int dmg)
    {
        int blocked = Mathf.Min(CurrentArmor, dmg);
        CurrentArmor -= blocked;
        OnArmorChanged?.Invoke(CurrentArmor);

        int remaining = dmg - blocked;
        
        CurrentHp -= remaining;
        OnHpChanged?.Invoke(CurrentHp);

        if (CurrentHp <= 0)
            Die();
    }

    private void Die()
    {
        CurrentHp = 0;
        EnemyManager.Instance.OnEnemyDead(this);
    }

    public void TurnAction(Player player)
    {
        if (IsDead)
            return;

        ActionType action = Data._ActionType[actionIndex];

        switch (action)
        {
            case ActionType.Attack:
                player.TakeDamage(Data.Dmage);
                break;
            case ActionType.Skill:
                break;
            case ActionType.Armor:
                CurrentArmor += Data.ArmorPower;
                OnArmorChanged?.Invoke(CurrentArmor);
                break;
        }

        actionIndex = (actionIndex + 1) % Data._ActionType.Length;
    }

    public void ApplyDebuff(CardData card)
    {
        if (card == null)
            return;

        switch (card._DeBuffType)
        {
            case DebuffType.None:
                break;
            case DebuffType.Weakness:
                int reduceAmount = card.Power;
                CurrentArmor = Mathf.Max(CurrentArmor - reduceAmount, 0);
                break;
        }
    }
}
