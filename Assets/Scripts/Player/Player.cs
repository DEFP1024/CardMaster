using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    private int hp;
    private int ap;
    private int money;
    private int armor;
    private int draw;

    public int Hp => hp;
    public int MaxAP => ap;
    public int AP => ap;
    public int Money => money;
    public int Armor => armor;
    public int Draw => draw;
    

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        hp = playerData.Hp;
        ap = playerData.AP;
        money = playerData.Money;
        armor = playerData.Armor;
        draw = playerData.Draw;
    }
}
