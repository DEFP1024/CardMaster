using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    private int hp;
    private int ap;
    private int maxAP;
    private int money;
    private int armor;
    private int draw;

    public PlayerData PlayerData => playerData;
    public int Hp => hp;
    public int MaxAP => maxAP;
    public int AP => ap;
    public int Money => money;
    public int Armor => armor;
    public int Draw => draw;

    public event Action<int> OnAPChanged;
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnHpChanged;
    public event Action<int> OnArmorChanged;


    private void Awake()
    {
        LoadData();
        GameManager.Instance.SetUpPlayer();
    }

    private void LoadData()
    {
        hp = playerData.Hp;
        maxAP = playerData.AP;
        ap = maxAP;
        money = playerData.Money;
        armor = playerData.Armor;
        draw = playerData.Draw;

        OnAPChanged?.Invoke(ap);
        OnMoneyChanged?.Invoke(money);
        OnHpChanged?.Invoke(hp);
        OnArmorChanged?.Invoke(armor);

    }

    public void UseAP(int amount)
    {
        ap -= amount;
        OnAPChanged?.Invoke(ap);
    }

    public void GainAP(int amount)
    {
        ap += amount;
        OnAPChanged?.Invoke(ap);
    }

    public void ResetAP()
    {
        ap = maxAP;
        OnAPChanged?.Invoke(ap);
    }

    public void Heal(int amount)
    {
        hp += amount;
        OnHpChanged?.Invoke(hp);
    }

    public void TakeDamage(int dmg)
    {
        if (dmg <= 0)
            return;

        int blocked = Mathf.Min(armor, dmg);
        armor -= blocked;

        int remainig = dmg - blocked;

        if (remainig > 0)
        {
            hp -= remainig;
        }

        OnArmorChanged?.Invoke(armor);
        OnHpChanged?.Invoke(hp);

        if (hp <= 0)
        {
            hp = 0;
            GameManager.Instance.HandlePlayerDead();
        }
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }

    public void ChangeArmor(int amount)
    {
        armor += amount;
        OnArmorChanged?.Invoke(armor);
    }

    public void ResetArmor()
    {
        armor = 0;
        OnArmorChanged?.Invoke(armor);
    }
}
