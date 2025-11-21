using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField] private CardtestObj testCardPrefab;  // 프리팹 기준(종류) 카드


    private List<CardtestObj> activeCards = new List<CardtestObj>();

    private void Start()
    {
        Debug.Log("=== 재고 초기 상태 ===");
        PrintRemain();

        Debug.Log("=== 3장 가져오기 (풀에서 꺼내서 활성화) ===");
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();

        Debug.Log("=== 3장 가져온 후 남은 장수 ===");
        PrintRemain();

        Debug.Log("=== 카드 1장 반환 ===");
        ReturnOneCard();
        PrintRemain();

        Debug.Log("=== 다시 2장 가져오기 ===");
        TestTakeCard();
        TestTakeCard();

        Debug.Log("=== 최종 남은 장수 ===");
        PrintRemain();
    }

    void TestTakeCard()
    {
        var instance = CardStockManager.Instance.TryTakeCard(testCardPrefab);

        if (instance != null)
        {
            Debug.Log($"카드 '{testCardPrefab.Data.Name}' → 1장 가져와서 활성화했습니다. 인스턴스: {instance.name}");
            instance.transform.SetParent(transform, false);
            activeCards.Add(instance);
        }
        else
            Debug.Log($"카드 '{testCardPrefab.Data.Name}' → 풀에 남은 카드가 없습니다!");
    }
    void ReturnOneCard()
    {
        if (activeCards.Count == 0)
        {
            Debug.Log("반환할 카드가 없습니다.");
            return;
        }

        // 마지막 카드 하나 꺼내서
        var last = activeCards[activeCards.Count - 1];
        activeCards.RemoveAt(activeCards.Count - 1);

        // 풀로 반환
        CardStockManager.Instance.ReturnCard(last);
    }

    void PrintRemain()
    {
        int remain = CardStockManager.Instance.GetRemainCount(testCardPrefab);
        Debug.Log($"'{testCardPrefab.Data.Name}' 남은 장수(비활성 상태): {remain}");
    }
}
