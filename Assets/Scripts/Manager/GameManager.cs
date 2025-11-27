using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    None,
    PlayerTurn,
    EnemyTurn
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerGrave graveParent;

    public Player Player => player;
    public int PlayerAP => player.AP;
    public int PlayerHP => player.Hp;

    public int PlayerMoney => player.Money;

    public int PlayerArmor => player.Armor;

    public GamePhase Phase { get; private set; } = GamePhase.None;

    public bool IsBattle => Phase != GamePhase.None;
    public bool IsPlayerTurn => Phase == GamePhase.PlayerTurn;

    private bool isRoutineTurn = false;

    protected override void Awake()
    {
        base.Awake();

        Instantiate(player,transform);


        if (player != null)
        {
            player.OnAPChanged += HandleAPChanged;
        }
    }

    public void SetUpPlayer()
    {
        player = FindFirstObjectByType<Player>();
        player.transform.SetParent(transform, false);
        player.OnAPChanged += HandleAPChanged;
    }
    public void SetUpGrave()
    {
        graveParent = FindFirstObjectByType<PlayerGrave>();
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnAPChanged -= HandleAPChanged;
        }
    }

    public void StartPlayerTurn()
    {
        Phase = GamePhase.PlayerTurn;

        player.ResetAP();

        var playerHand = FindFirstObjectByType<PlayerHand>();

        for (int i = 0; i < player.Draw; i++)
        {
            playerHand.CardDraw();
        }

        player.ResetArmor();

        HandManager.Instance.RefreshHandCards();

        HandManager.Instance.UpdateHighlights(player.AP);
    }

    public void EndPlayerTurn()
    {
        Phase = GamePhase.EnemyTurn;
    }

    private IEnumerator EnemyTurnRoutine()
    {
        foreach (var enemy in EnemyManager.Instance.ActiveEnemys)
        {
            if (enemy == null || enemy.IsDead)
                continue;

            enemy.TurnAction(player);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnClickEndTurn()
    {
        
        if (IsPlayerTurn == false || IsBattle == false)
            return;
        if (isRoutineTurn)
            return;
        Debug.Log("ео а╬╥А╣й");
        StartCoroutine(EndTurnSequence());
    }

    private IEnumerator EndTurnSequence()
    {
        isRoutineTurn = true;

        HandManager.Instance.AllGrave();

        EndPlayerTurn();

        yield return StartCoroutine(EnemyTurnRoutine());

        if (player.Hp <= 0)
        {
            HandlePlayerDead();
            isRoutineTurn = false;
            yield break;
        }

        if (EnemyManager.Instance != null && EnemyManager.Instance.ActiveEnemys.Count == 0)
        {
            EndBattle();
            isRoutineTurn = false;
            yield break;
        }

        StartPlayerTurn();

        isRoutineTurn = false;
    }

    private void HandleAPChanged(int currentAP)
    {
        if (IsBattle == false)
            return;

        HandManager.Instance.UpdateHighlights(currentAP);
    }

    public bool TryPlayCard(CardObj card)
    {
        if (card == null || card.Data == null)
            return false;

        var data = card.Data;

        if (Phase != GamePhase.PlayerTurn)
            return false;

        if (card.Cost > player.AP)
            return false;

        switch (data.CardTarget)
        {
            case CardTaget.Self:
                player.UseAP(card.Cost);
                UseSelf(card);
                HandManager.Instance.RemoveHand(card);
                MoveGrave(card);
                return true;

            case CardTaget.EnemyAll:
                player.UseAP(card.Cost);
                UseEnemyAll(card);
                HandManager.Instance.RemoveHand(card);
                MoveGrave(card);
                return true;

            case CardTaget.None:
                player.UseAP(card.Cost);
                HandManager.Instance.RemoveHand(card);
                MoveGrave(card);
                return true;

            default:
                return false;
        }
    }

    public bool TryPlayCard(CardObj card, EnemyUnit targetEnemy)
    {
        if (card == null || card.Data == null)
            return false;

        var data = card.Data;

        if (Phase != GamePhase.PlayerTurn)
            return false;

        if (card.Cost > player.AP)
            return false;

        if (targetEnemy == null)
            return false;

        if (data.CardTarget != CardTaget.Enemy)
        {
            return TryPlayCard(card);
        }

        player.UseAP(card.Cost);
        HandManager.Instance.RemoveHand(card);
        MoveGrave(card);

        UseEnemy(card, targetEnemy);

        return true;
    }

    private void UseSelf(CardObj card)
    {
        var data = card.Data;

        if (data.Type == CardType.Skill)
        {
            if (data._BuffType != BuffType.None)
            {
                switch (data._BuffType)
                {
                    case BuffType.Heal:
                        player.Heal(data.Power);
                        break;
                }
            }
        }
        else if (data.Type == CardType.Armor)
        {
            player.ChangeArmor(data.Power);
        }
    }

    private void UseEnemy(CardObj card, EnemyUnit enemy)
    {
        var data = card.Data;

        if (data.Type == CardType.Attack)
        {
            enemy.TakeDamage(data.Power);
        }
        else if (data.Type == CardType.Skill)
        {
            enemy.ApplyDebuff(data);
        }
    }

    private void UseEnemyAll(CardObj card)
    {
        var data = card.Data;

        if (EnemyManager.Instance == null)
            return;

        var enemys = new List<EnemyUnit>(EnemyManager.Instance.ActiveEnemys);

        foreach (var enemy in enemys)
        {
            if (enemy == null) continue;

            if (data.Type == CardType.Attack)
            {
                enemy.TakeDamage(data.Power);
            }
            else if (data.Type == CardType.Skill)
            {
                enemy.ApplyDebuff(data);
            }
        }
    }
    public void EndBattle()
    {
        Phase = GamePhase.None;
        
        var pDeck = FindFirstObjectByType<PlayerDeck>();
        var pGrave = FindFirstObjectByType<PlayerGrave>();

        HandManager.Instance.AllGrave();
        pDeck.AllReturnCardDeck();
        pGrave.AllReturnCardDeck();

        EnemyManager.Instance.ResetAll();

        if (HandManager.Instance != null)
            Destroy(HandManager.Instance.gameObject);
        SceneChanger.Instance.OnStage();
    }

    public void ChangePlayerHP(int amount)
    {
        if (player == null)
            return;

        player.TakeDamage(amount);
    }

    public void ChangePlayerMoney(int amount)
    {
        if (player == null)
            return;

        player.ChangeMoney(amount);
    }

    public bool TrySpendMoney(int amount)
    {
        if (player == null)
            return false;

        if (player.Money < amount)
            return false;

        player.ChangeMoney(-amount);
        return true;
    }

    public void ChangePlayerArmor(int amount)
    {
        if (player == null)
            return;

        player.ChangeArmor(amount);
    }

    public void StartBattle(NodeType nodeType)
    {
        Phase = GamePhase.PlayerTurn;

        MonsterType type = (nodeType == NodeType.Boss) ? MonsterType.Boss : MonsterType.Enemy;
        EnemyManager.Instance.SpawnEnemy(type);

        StartPlayerTurn();
    }

    public void HandlePlayerDead()
    {
        Phase = GamePhase.None;

        SceneChanger.Instance.OnDefeate();
    }

    public void MoveGrave(CardObj card)
    {
        card.InHand = false;
        card.RemoveHighlight();

        if (graveParent != null)
            card.transform.SetParent(graveParent.transform.GetChild(0).GetChild(0), false);

        graveParent.AddCard(card);
        card.gameObject.SetActive(false);
    }

    public void HandleBossClear()
    {
        var pDeck = FindFirstObjectByType<PlayerDeck>();
        var pGrave = FindFirstObjectByType<PlayerGrave>();

        HandManager.Instance.AllGrave();
        pDeck.AllReturnCardDeck();
        pGrave.AllReturnCardDeck();

        EnemyManager.Instance.ResetAll();

        if (HandManager.Instance != null)
            Destroy(HandManager.Instance.gameObject);
        SceneChanger.Instance.OnVictory();
    }
}
