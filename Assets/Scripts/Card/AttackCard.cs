using UnityEngine;

public class AttackCard : MonoBehaviour
{
    [SerializeField] private CardData card;

    public void UseCard()
    {
        switch (card.Type)
        {
            case CardType.Attack:
                break;
            case CardType.Armor:
                break;
            case CardType.Skill:
                break;
        }
    }
    
    void UseAttackCard()
    {
        {
            var enemy = FindAnyObjectByType<CardData>();
            if (enemy == null)
                return;

        }
    }
}
