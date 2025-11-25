using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject, ICard
{
    [Header("기본 정보")]
    [Tooltip("고유 번호")]
    [SerializeField] private int id;
    [Tooltip("카드 이름")]
    [SerializeField] private string cardName;
    [Tooltip("카드 타입")]
    [SerializeField] private CardType cardType;
    [Tooltip("카드 코스트")]
    [Min(0)]
    [SerializeField] private int cost;
    [Tooltip("카드 힘(공격력, 아머수치, 스킬 파워)")]
    [SerializeField] private int power;
    [Tooltip("카드 설명")]
    [SerializeField] private string description;
    [Tooltip("카드 이미지")]
    [SerializeField] private Sprite cardImage;


    public int ID => id;

    public string Name => cardName;

    public int Cost => cost;

    public int Power => power;

    public string Description => description;

    public CardType Type => cardType;

    public Sprite CardImage => cardImage;

}
