using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField] private CardData testCard;

    private void Start()
    {
        // 1. 시작할 때 해당 카드가 몇 장 있는지 출력
        Debug.Log("=== 재고 초기 상태 ===");
        PrintRemain();

        // 2. 3장 가져오기
        Debug.Log("=== 3장 가져오기 ===");
        TestTakeCard();
        TestTakeCard();
        TestTakeCard();

        // 3. 현재 남은 장수 출력
        Debug.Log("=== 3장 가져온 후 남은 장수 ===");
        PrintRemain();

        // 4. 카드 1장 되돌리기
        Debug.Log("=== 카드 1장 반환 ===");
        CardStockManager.Instance.ReturnCard(testCard);
        PrintRemain();

        // 5. 다시 2장 가져오기
        Debug.Log("=== 다시 2장 가져오기 ===");
        TestTakeCard();
        TestTakeCard();

        // 6. 최종 남은 장수 출력
        Debug.Log("=== 최종 남은 장수 ===");
        PrintRemain();
    }

    void TestTakeCard()
    {
        bool result = CardStockManager.Instance.TryTakeCard(testCard);

        if (result)
            Debug.Log($"카드 '{testCard.Name}' → 1장 가져왔습니다.");
        else
            Debug.Log($"카드 '{testCard.Name}' → 재고가 없습니다!");
    }

    void PrintRemain()
    {
        int remain = CardStockManager.Instance.GetRemainCount(testCard);
        Debug.Log($"'{testCard.Name}' 남은 장수: {remain}");
    }
}